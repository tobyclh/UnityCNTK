using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System.IO;
using DeepUnity;
namespace DeepUnity
{
    public class test : MonoBehaviour
    {

        Function model;
        public Texture2D styleRef;
        public Texture2D src;
        // Use this for initialization
        void Start()
        {

            //load model
            string modelPath = System.IO.Path.Combine(Environment.CurrentDirectory, @"Assets\CNTK\model.dnn");
            model = Function.LoadModel(modelPath, DeviceDescriptor.CPUDevice);

            Evaluate(src, styleRef);
        }

        public Texture2D Evaluate(Texture2D source, Texture2D style)
        {
            string inputOneHotString = "";
            // Get input variable
            var inputVar = model.Arguments.Single();
            // Get shape 

            var inputDataMap = new Dictionary<Variable, Value>();
            // inputDataMap.Add(inputVar, Value.Create(source));

            // Prepare output
            Variable outputVar = model.Output;

            // Create ouput data map. Using null as Value to indicate using system allocated memory.
            var outputDataMap = new Dictionary<Variable, Value>();
            outputDataMap.Add(outputVar, null);

            // Evaluate the model.
            model.Evaluate(inputDataMap, outputDataMap, DeviceDescriptor.CPUDevice);

            // Get output result
            Value outputVal = outputDataMap[outputVar];
            var outputData = outputVal.GetDenseData<float>(outputVar);
            Color[] colorArray = new Color[source.width * source.height];
            for (int i = 0; i < source.width * source.height; i++)
            {
                var color = colorArray[i];
                color.r = outputData[0][i];
                color.g = outputData[1][i];
                color.b = outputData[2][i];
            }
            Texture2D outputTexture = new Texture2D(source.width, source.height);
            outputTexture.SetPixels(colorArray);
            return outputTexture;
        }

    }

}
