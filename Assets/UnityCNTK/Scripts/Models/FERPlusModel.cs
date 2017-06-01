using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityCNTK;
using CNTK;
using System;
using UnityEngine.Assertions;

namespace UnityCNTK
{

    public class FERPlusModel : Model<Texture2D, int>, IStreamModel
    {
        [Tooltip("Evaluation carry out in every specificed second")]
        public float evaluationPeriod = 10;
        private bool shouldStop = false;
        private bool isStreaming = false;
        public new Texture2DSource dataSource;
        public void StartStreaming()
        {
            Assert.IsFalse(isStreaming, " is already streaming");
            Debug.Log("Start Streaming");
            InvokeRepeating("MyStream", 0, evaluationPeriod);
        }

        private void MyStream()
        {
            Debug.Log("Grabbing data");
            var data =dataSource.GetData();
            var s = Evaluate(data);
        }

        public void StopStreaming()
        {
            Assert.IsTrue(isStreaming, name + " is not streaming");
            CancelInvoke("MyStream");
        }
        public override int OnPostProcess(Value output)
        {
            Debug.Log("OnPostprocess FER+");
            throw new NotImplementedException();
        }

        public override Value OnPreprocess(Texture2D input)
        {
            Debug.Log("OnPreprocess FER+");
            return input.ToValue(CNTKManager.device);
        }


    }
}