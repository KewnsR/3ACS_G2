
using Microsoft.AspNetCore.Hosting;
using Microsoft.ML.OnnxRuntime;
using System;
using System.IO;

namespace HumanRepProj.Services
{

    public interface IOnnxFaceDetectionService
    {
        InferenceSession Session { get; }
    }

    public class OnnxFaceDetectionService : IOnnxFaceDetectionService
    {
        private readonly Lazy<InferenceSession> _session;

        public OnnxFaceDetectionService(IWebHostEnvironment env)
        {
            var modelPath = Path.Combine(env.WebRootPath, "models", "yolov8n-face.onnx");

            if (!File.Exists(modelPath))
                throw new FileNotFoundException($"ONNX model not found at: {modelPath}");

            _session = new Lazy<InferenceSession>(() =>
                new InferenceSession(modelPath, new Microsoft.ML.OnnxRuntime.SessionOptions
                {
                    ExecutionMode = ExecutionMode.ORT_SEQUENTIAL
                }));
        }

        public InferenceSession Session => _session.Value;
    }
}
