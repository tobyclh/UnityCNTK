using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CNTK;
using System.Timers;
using System.Threading;
using CNTK;
namespace UnityCNTK
{
    // Unifying class that combine the use of both EvalDLL and CNTKLibrary
    // The reason we have not dropped support for EvalDLL is that it expose more native function of the full API,
    // which are very useful in general.
    public abstract class Model : ScriptableObject
    {   
        public Function function;
        public IConvertible input;
        public IConvertible output;
        public Thread thread; 
        public bool KeepModelLoaded = false;
        public abstract void LoadModel();

        public abstract void Evaluate(DeviceDescriptor device);

        public abstract void OnEvaluated();

    }

}
