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

        public override void Evaluate(DeviceDescriptor device)
        {
            throw new NotImplementedException();
        }

        public override void LoadModel()
        {
            throw new NotImplementedException();
        }
    }
}