using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;
using UnityEngine.Assertions;
using CNTK;
using UnityEngine.UI;
namespace UnityCNTK
{
    public abstract class DataSource<T>:MonoBehaviour
    {
        public delegate T getData();
        public getData GetData;
        
        private Rigidbody rb;
        private CNTKManager manager;

    }

}
