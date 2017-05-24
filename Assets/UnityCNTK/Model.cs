using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq;
using UnityEngine;
using UnityEditor;
using CNTK;
using System.Timers;
namespace UnityCNTK
{
    public class Model : ScriptableObject
    {   
        public Function function;
    }
}