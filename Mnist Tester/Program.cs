using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using CNTK;

namespace Mnist_Tester
{
    internal static class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Loading CNTK Model...");
            var device = DeviceDescriptor.UseDefaultDevice();
            while (true)
            {
                var grayscale = LoadImage();

                var model = Function.Load("Model/model", device);

                var inputVar = model.Arguments.Single();
                var inputShape = inputVar.Shape;

                var outputVar = model.Output;
                var outputDataMap = new Dictionary<Variable, Value> {{outputVar, null}};

                var inputDataMap = new Dictionary<Variable, Value>();
                var inputVal = Value.CreateBatch(inputShape, grayscale, device);
                inputDataMap.Add(inputVar, inputVal);

                model.Evaluate(inputDataMap, outputDataMap, device);

                var outputVal = outputDataMap[outputVar];
                var outputData = outputVal.GetDenseData<float>(outputVar);
                var resultVector = outputData[0];
                PrintResult(resultVector);
                
                Console.WriteLine("Press 1 to evaluate again or any other key to exit...");
                var consoleKeyInfo = Console.ReadKey(true);
                if (consoleKeyInfo.KeyChar != '1')
                {
                    break;
                }
                Console.Clear();
            }
        }

        private static IEnumerable<float> LoadImage()
        {
            var image = Image.FromFile("mnist_digit.png");
            var bitmap = new Bitmap(image);
            if (bitmap.Height != 28 || bitmap.Width != 28)
            {
                throw new FormatException("The image should have 28x28 dimensions.");
            }

            var result = ToGrayscale(bitmap);
            
            bitmap.Dispose();
            image.Dispose();

            return result;
        }

        private static IEnumerable<float> ToGrayscale(Bitmap bmp) 
        {
            var result = new List<float>();

            for (var y = 0; y < bmp.Height; y++) 
            {
                for (var x = 0; x < bmp.Width; x++) 
                {
                    var c = bmp.GetPixel(x, y);
                    var rgb = (float) (0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B);
                    result.Add(rgb);
                }
            }

            return result;
        }

        private static void PrintResult(IList<float> resultVector)
        {
            var maxIndex = 0;
            for (var i = 1; i < resultVector.Count; i++)
            {
                if (resultVector[i] > resultVector[maxIndex])
                {
                    maxIndex = i;
                }
            }
            Console.WriteLine($"You wrote: {maxIndex}");
        }
    }
}