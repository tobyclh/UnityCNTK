using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace UnityCNTK
{
    public class DataSource : MonoBehaviour
    {
        public Object source;
        public enum PredefinedSource
        {
            none, pos, rot, quat, velocity, acceleration
        }

        private CNTKManager manager;
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
