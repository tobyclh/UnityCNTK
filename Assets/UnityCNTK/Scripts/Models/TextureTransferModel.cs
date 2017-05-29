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
    [CreateAssetMenu(fileName = "TextureModel", menuName = "UnityCNTk/Model/TextureModel")]
    public class TextureModel : Model
    {
        public new string relativeModelPath = "Assets/UnityCNTK/Model/VGG.dnn";
        public double contentWeight = 5;
        public double styleWeight = 1;
        public double decay = 0.5f;
        public int width = 50;
        public int height = 50;
        // Use this for initialization
        
        //post processing output data
        protected override void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            List<Texture2D> textures = new List<Texture2D>();
            // function.Evaluate(new Dictionary<Variable, Value>().Add(inputVar,))   
            Texture2D outputTexture = new Texture2D(width, height);
            output = textures;
        }

    }

}
