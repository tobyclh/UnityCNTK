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
        public string relativeModelPath = "Assets/UnityCNTK/Model/VGG.dnn";

        public Texture2D styleRef;

        public new IEnumerable<Texture2D> input;
        public new IEnumerable<Texture2D> output;

        // Use this for initialization
        void Start()
        {
            LoadModel();
        }

        public override void Evaluate(DeviceDescriptor device)
        {
            input.ToValue(device, false);
            var shape = function.Arguments.Single().Shape;
            styleRef.ResampleAndCrop(shape[0], shape[1]);
            foreach(var img in input)
            {
                img.ResampleAndCrop(shape[0], shape[1]);
            }
            var styleValue = new List<Texture2D>() { styleRef }.ToValue(device, false);
            List<Texture2D> textures = new List<Texture2D>();
            Texture2D outputTexture = new Texture2D(input.FirstOrDefault().width, input.FirstOrDefault().height);
            output = textures;
        }

        public override void LoadModel()
        {
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            Downloader.DownloadPretrainedModel(Downloader.pretrainedModel.VGG16, absolutePath);
            
        }

        // Background thread that does the heavy lifting
        public void NeuralAlgorithmOfArtisticStlyeImproved(Value source, Value style)
        {
            float decay = 0.5f;

            
            var inputVar = function.Arguments.Single();
            var inputDataMap = new Dictionary<Variable, Value>();
            inputDataMap.Add(inputVar, source);
            // Prepare output
            Variable outputVar = function.Output;

            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);

            // Evaluate the model.
            function.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);
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
