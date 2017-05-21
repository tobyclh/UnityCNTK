using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityCNTK;
using System.Net;
using System.Threading;
namespace UnityCNTK
{
    public class TextureTransferModel : Model
    {
        /* The implementation is a C# version of the original implementation found in https://github.com/Microsoft/CNTK/blob/master/Tutorials/CNTK_205_Artistic_Style_Transfer.ipynb
        This is by no mean the optimized version*/
        public string relativeModelPath = @"Assets\UnityCNTK\Model\VGG.dnn";

        public Texture2D styleRef;
        public Texture2D src;
        // Use this for initialization
        Function model;
        void Start()
        {
            LoadModel();
        }



        public Texture2D CreateStylizedImage(Texture2D source, Texture2D style)
        {
            var srcValue = Convert.ToValue(source, false);
            var styleValue = Convert.ToValue(style, false);

            Texture2D outputTexture = new Texture2D(source.width, source.height);
            // outputTexture.SetPixels(colorArray);
            return outputTexture;
        }
        /// Load model into function, download if not exist

        private void LoadModel()
        {
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            Downloader.DownloadPretrainedModel(Downloader.pretrainedModel.VGG16, absolutePath);
            model = Function.LoadModel(absolutePath, DeviceDescriptor.GPUDevice(0));
        }

        // Background thread that does the heavy lifting
        public void NeuralAlgorithmOfArtisticStlyeImproved(Value source, Value style)
        {
            var inputVar = model.Arguments.Single();
            var inputDataMap = new Dictionary<Variable, Value>();
            inputDataMap.Add(inputVar, source);
            // Prepare output
            Variable outputVar = model.Output;

            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);

            // Evaluate the model.
            model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);
            // Get output result
            Value outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);
            Color[] colorArray = new Color[source.Shape.GetDimensionSize(1) * source.Shape.GetDimensionSize(2)];
            for (int i = 0; i < source.Shape.GetDimensionSize(1) * source.Shape.GetDimensionSize(2); i++)
            {
                var color = colorArray[i];
                color.r = outputData[0][i];
                color.g = outputData[1][i];
                color.b = outputData[2][i];
            }
            

        }




    }

}
