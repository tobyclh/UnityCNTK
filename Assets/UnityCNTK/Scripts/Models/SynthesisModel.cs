using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CNTK;
using System;
namespace UnityCNTK
{
    public class SynthesisModel : Model
    {
        public Texture2D textureReference;
        
        public string namingRules;

        public override void Evaluate()
        {
            throw new NotImplementedException();
        }

        protected override void OnEvaluated(Dictionary<Variable, Value> outputDataMap)
        {
            throw new NotImplementedException();
        }
    }
}