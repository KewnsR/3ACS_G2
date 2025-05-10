﻿using Microsoft.AspNetCore.Mvc;
using HumanRepProj.Data;
using HumanRepProj.Models;
using Microsoft.ML.OnnxRuntime;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML.OnnxRuntime.Tensors;
using static System.Net.Mime.MediaTypeNames;
using HumanRepProj.Services;

namespace HumanRepProj.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AttendanceController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly IOnnxFaceDetectionService _faceDetectionService;
        private readonly FaceRecognitionService _faceRecognitionService; // Add this

        public AttendanceController(
            ApplicationDbContext context,
            IOnnxFaceDetectionService faceDetectionService,
            FaceRecognitionService faceRecognitionService) // Inject service
        {
            _context = context;
            _faceDetectionService = faceDetectionService;
            _faceRecognitionService = faceRecognitionService;
        }

        [HttpPost("log")]
        public async Task<IActionResult> LogAttendance([FromBody] AttendanceDto dto)
        {
            var employee = await _context.Employees.FindAsync(dto.EmployeeId);
            if (employee == null)
                return NotFound("Employee not found");

            var attendance = new AttendanceRecord
            {
                EmployeeID = dto.EmployeeId,
                AttendanceDate = DateTime.Today,
                Status = "Present",
                TimeIn = DateTime.Now.TimeOfDay,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };

            _context.AttendanceRecords.Add(attendance);
            await _context.SaveChangesAsync();

            return Ok(new { success = true });
        }

        [HttpPost("verify-face")]
        public async Task<IActionResult> VerifyFace([FromBody] FaceVerifyDto dto)
        {
            // 1. Face Detection
            var detectionResult = await DetectFace(dto.Image);
            var detectionValue = detectionResult.Value;
            if (detectionValue == null || !detectionValue.Faces.Any())
                return BadRequest("No faces detected");


            // 2. Get Employee Face Data
            var employee = await _context.Employees
                .Include(e => e.FaceData)
                .FirstOrDefaultAsync(e => e.EmployeeID == dto.EmployeeId);

            if (employee?.FaceData == null)
                return NotFound("Employee face data not found");

            // 3. Face Comparison (Simplified - implement actual comparison)
            var faceData = employee.FaceData as FaceData;
            var match = CompareFaces(faceData?.OriginalImage, dto.Image);

            return Ok(new FaceVerifyResult
            {
                Match = match,
                Confidence = match ? 0.95f : 0.1f // Example values
            });
        }

        [HttpPost("detect-face")]
        public async Task<ActionResult<FaceDetectionResult>> DetectFace([FromBody] string imageData)
        {
            try
            {
                // Convert base64 to image
                var imageBytes = Convert.FromBase64String(imageData.Split(',').Last());
                using var ms = new MemoryStream(imageBytes);
                using var image = SixLabors.ImageSharp.Image.Load<Rgb24>(ms);

                // Preprocess image for YOLO
                var inputTensor = PreprocessImage(image);

                // Create input for ONNX
                var inputs = new List<NamedOnnxValue>
                {
                    NamedOnnxValue.CreateFromTensor("images", inputTensor)
                };

                // Run inference
                var results = _faceDetectionService.Session.Run(inputs);
                var output = results.First().AsTensor<float>(); // Add this
                var faces = ParseYoloOutput(output); // Use parsed output

                return new FaceDetectionResult
                {
                    Success = true,
                    Faces = faces,
                    Message = $"Detected {faces.Count} faces"
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new FaceDetectionResult
                {
                    Success = false,
                    Message = ex.Message
                });
            }
        }

        private bool CompareFaces(string referenceImageBase64, string testImageBase64)
        {
            var referenceEmbedding = GetFaceEmbedding(referenceImageBase64);
            var testEmbedding = GetFaceEmbedding(testImageBase64);

            float similarity = CosineSimilarity(referenceEmbedding, testEmbedding);
            return similarity > 0.6f;
        }

        private float[] GetFaceEmbedding(string base64Image)
        {
            try
            {
                var imageBytes = Convert.FromBase64String(base64Image.Split(',').Last());
                return _faceRecognitionService.GetFaceEmbedding(imageBytes); // Use injected service
            }
            catch
            {
                return null;
            }
        }

        private float CosineSimilarity(float[] vectorA, float[] vectorB)
        {
            if (vectorA == null || vectorB == null || vectorA.Length != vectorB.Length)
                return 0f;

            float dot = 0f, magA = 0f, magB = 0f;
            for (int i = 0; i < vectorA.Length; i++)
            {
                dot += vectorA[i] * vectorB[i];
                magA += vectorA[i] * vectorA[i];
                magB += vectorB[i] * vectorB[i];
            }

            return dot / (float)(Math.Sqrt(magA) * Math.Sqrt(magB));
        }


        private Tensor<float> PreprocessImage(Image<Rgb24> image)
        {
            const int targetSize = 640;
            // Resize
            using var resized = image.Clone(x => x.Resize(targetSize, targetSize));
            // Normalize and convert to tensor [1, 3, 640, 640]
            var tensor = new DenseTensor<float>(new[] { 1, 3, targetSize, targetSize });
            for (int y = 0; y < resized.Height; y++)
            {
                for (int x = 0; x < resized.Width; x++)
                {
                    var pixel = resized[x, y];
                    tensor[0, 0, y, x] = pixel.R / 255f; // Red
                    tensor[0, 1, y, x] = pixel.G / 255f; // Green
                    tensor[0, 2, y, x] = pixel.B / 255f; // Blue
                }
            }
            return tensor;
        }

        private List<FaceBox> ParseYoloOutput(Tensor<float> output)
        {
            const float confidenceThreshold = 0.5f;
            var detectedFaces = new List<FaceBox>();

            // YOLOv8 output shape: [batch, num_anchors, 16] where 16 = [cx, cy, w, h, angle, conf, 10 landmarks]
            for (int i = 0; i < output.Dimensions[1]; i++) // Iterate over anchors
            {
                float confidence = output[0, i, 4]; // Confidence score
                if (confidence >= confidenceThreshold)
                {
                    float cx = output[0, i, 0];
                    float cy = output[0, i, 1];
                    float width = output[0, i, 2];
                    float height = output[0, i, 3];

                    // Convert to top-left + width/height
                    float x = cx - width / 2;
                    float y = cy - height / 2;

                    detectedFaces.Add(new FaceBox
                    {
                        X = x,
                        Y = y,
                        Width = width,
                        Height = height,
                        Confidence = confidence
                    });
                }
            }
            return detectedFaces;
        }
    }

    public class AttendanceDto
    {
        public int EmployeeId { get; set; }
    }

    public class FaceVerifyDto
    {
        public int EmployeeId { get; set; }
        public string Image { get; set; }
    }

    public class FaceVerifyResult
    {
        public bool Match { get; set; }
        public float Confidence { get; set; }
    }

    public class FaceDetectionResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public List<FaceBox> Faces { get; set; }
    }

    public class FaceBox
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Confidence { get; set; }
    }
    public class FaceData
    {
        public int FaceDataID { get; set; }
        public int EmployeeID { get; set; }
        public string OriginalImage { get; set; } // base64 string or URL
    }

}