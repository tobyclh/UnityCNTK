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

        public new IEnumerable<Texture2D> input;
        public new IEnumerable<Texture2D> output;
        // Use this for initialization
        Function model;
        void Start()
        {
            LoadModel();
        }

        public override void Evaluate()
        {
            input.ToValue(false);
            var styleValue = new List<Texture2D>() { styleRef }.ToValue(false);
            List<Texture2D> textures = new List<Texture2D>();
            Texture2D outputTexture = new Texture2D(input.FirstOrDefault().width, input.FirstOrDefault().height);
            // outputTexture.SetPixels(colorArray);
            output = textures;
            
        }
        /// Load model into function, download if not exist

        public override void LoadModel()
        {
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            Downloader.DownloadPretrainedModel(Downloader.pretrainedModel.VGG16, absolutePath);
            
        }

        // Background thread that does the heavy lifting
        public void NeuralAlgorithmOfArtisticStlyeImproved(Value source, Value style)
        {
            float decay = 0.5f;

            
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
            var texture = Convert.ToTexture2D(outputVal, outputVar);

        }

        public double[] ObjectiveFunction()
        {
            
            return new double[2];
        }

        

    }

}
