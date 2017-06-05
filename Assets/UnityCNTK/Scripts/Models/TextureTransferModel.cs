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
    public class TextureModel<U,V> : Model<U, V>
    {
        public new string relativeModelPath = "Assets/UnityCNTK/Model/VGG.dnn";
        public double contentWeight = 5;
        public double styleWeight = 1;
        public double decay = 0.5f;
        public int width = 50;
        public int height = 50;
        

    }

}
