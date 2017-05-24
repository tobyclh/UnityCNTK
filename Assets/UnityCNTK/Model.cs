using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CNTK;
using System.Timers;
using CNTK.Extensibility.Managed;
namespace UnityCNTK
{
    public abstract class Model : ScriptableObject
    {   
        public Function function;

        public Object input;
        public Object output;
        public abstract void LoadModel();

        public abstract void Evaluate();

    }

}
