using Microsoft.ML.OnnxRuntime.Tensors;
using Microsoft.ML.OnnxRuntime;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace HumanRepProj.Services
{
    public class FaceRecognitionService
    {
        private readonly InferenceSession _session;

        public FaceRecognitionService(InferenceSession session)
        {
            _session = session;
        }

        public float[] GetFaceEmbedding(byte[] imageBytes)
        {
            using var ms = new MemoryStream(imageBytes);
            using var image = Image.Load<Rgb24>(ms);

            var inputTensor = PreprocessImageForFaceNet(image);
            var inputs = new List<NamedOnnxValue>
        {
            NamedOnnxValue.CreateFromTensor("input", inputTensor)
        };

            using var results = _session.Run(inputs);
            var embedding = results.First().AsTensor<float>().ToArray();
            return embedding;
        }

        private Tensor<float> PreprocessImageForFaceNet(Image<Rgb24> image)
        {
            const int targetSize = 112;

            var resized = image.Clone(
                Configuration.Default,
                operations => operations.Resize(targetSize, targetSize)
            );
            var tensor = new DenseTensor<float>(new[] { 1, 3, targetSize, targetSize });

            for (int y = 0; y < resized.Height; y++)
            {
                for (int x = 0; x < resized.Width; x++)
                {
                    var pixel = resized[x, y];
                    tensor[0, 0, y, x] = (pixel.R - 127.5f) / 127.5f;
                    tensor[0, 1, y, x] = (pixel.G - 127.5f) / 127.5f;
                    tensor[0, 2, y, x] = (pixel.B - 127.5f) / 127.5f;
                }
            }

            return tensor;
        }
    }
}