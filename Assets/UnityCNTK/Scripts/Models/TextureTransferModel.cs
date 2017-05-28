using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using UnityCNTK;
using System.Net;
using System.Threading; 
using UnityEngine.Assertions;
namespace UnityCNTK
{
    public class TextureTransferModel : Model
    {
        /* The implementation is a C# version of the original implementation found in https://github.com/Microsoft/CNTK/blob/master/Tutorials/CNTK_205_Artistic_Style_Transfer.ipynb
        This example shows the limitation of the evaluation API, that deviation of parameter cannot be accessed. Therefore, in terms of 
        optimisation this script adopts a gradient free optimisation, using Accord
        

        */
 
        public new string relativeModelPath = "Assets/UnityCNTK/Model/VGG.dnn";

        public Texture2D styleRef;
        public new IEnumerable<Texture2D> input;
        public new IEnumerable<Texture2D> output;

        public double contentWeight = 5;
        public double styleWeight = 1;
        public double decay = 0.5f;
        // Use this for initialization
        
        public override void LoadModel()
        {
            var absolutePath = System.IO.Path.Combine(Environment.CurrentDirectory, relativeModelPath);
            // Downloader.DownloadPretrainedModel(Downloader.pretrainedModel.VGG16, absolutePath);
        }

        // Background thread that does the heavy lifting

        public double[] ObjectiveFunction()
        {
            throw new NotImplementedException();
            // return new double[2];
        }

        //post processing output data
        protected override void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            List<Texture2D> textures = new List<Texture2D>();
            // function.Evaluate(new Dictionary<Variable, Value>().Add(inputVar,))   
            Texture2D outputTexture = new Texture2D(input.FirstOrDefault().width, input.FirstOrDefault().height);
            output = textures;
        }

        public override List<Dictionary<Variable, Value>> OnPreprocess()
        {
            throw new NotImplementedException();
            Assert.IsNotNull(device);
            Assert.IsNotNull(input);
            if (function == null) LoadModel();
            var inputVar = function.Arguments.Single();
            var inputDataMap = new Dictionary<Variable, Value>() { { inputVar, input.ToValue(device) } };
            var outputDataMap = new Dictionary<Variable, Value>() { { function.Output, null } };
            return new List<Dictionary<Variable, Value>>() { inputDataMap, outputDataMap };
        }
    }

}
