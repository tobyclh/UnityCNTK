using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityCNTK;
using System.Net;
using System.Threading; 
using Accord.Math.Optimization;
using Microsoft.MSR.CNTK.Extensibility.Managed;
namespace UnityCNTK
{
    public class TextureTransferModel : Model
    {
        /* The implementation is a C# version of the original implementation found in https://github.com/Microsoft/CNTK/blob/master/Tutorials/CNTK_205_Artistic_Style_Transfer.ipynb
        This example shows the limitation of the evaluation API, that deviation of parameter cannot be accessed. Therefore, in terms of 
        optimisation this script adopts a gradient free optimisation, using Accord
        

        */
 
        public string relativeModelPath = "Assets/UnityCNTK/Model/VGG.dnn";

        public Texture2D styleRef;
        public new IEnumerable<Texture2D> input;
        public new IEnumerable<Texture2D> output;

        public double contentWeight = 5;
        public double styleWeight = 1;
        public double decay = 0.5f;
        // Use this for initialization
        void Start()
        {
            LoadModel();
        }

        public override void Evaluate(DeviceDescriptor device)
        {
            input.ToValue(device);
            var styleVar = function.Arguments[0];
            var shape = styleVar.Shape;
            var resizedStyle = styleRef.ResampleAndCrop(shape[0], shape[1]);
            var outputVar = function.Output;
            var styleValue = new List<Texture2D>() { resizedStyle }.ToValue(device);
            var inputDataMap = new Dictionary<Variable, Value>(){{styleVar, styleValue}};
            var outputDataMap = new Dictionary<Variable, Value>(){{outputVar,null}};
            thread = new Thread(() => function.Evaluate(inputDataMap, outputDataMap, device));
            thread.Start();  
        }

        public override void LoadModel()
        {
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            // Downloader.DownloadPretrainedModel(Downloader.pretrainedModel.VGG16, absolutePath);
        }

        // Background thread that does the heavy lifting

        public double[] ObjectiveFunction()
        {
            
            return new double[2];
        }

        public override void OnEvaluated()
        {
            List<Texture2D> textures = new List<Texture2D>();
            // function.Evaluate(new Dictionary<Variable, Value>().Add(inputVar,))   
            Texture2D outputTexture = new Texture2D(input.FirstOrDefault().width, input.FirstOrDefault().height);
            output = textures;
            
        }
    }

}
