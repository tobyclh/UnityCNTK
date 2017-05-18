using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System;
namespace DeepUnity
{
    public static class Extensionas
    {
        public static Value Create(this Value value, Texture2D texture)
        {
            var shape = NDShape.CreateNDShape(new int[] { texture.width, texture.height, 3 });
            var rawData = texture.GetRawTextureData();
            NDArrayViewPtrVector pVec = new NDArrayViewPtrVector(rawData);
            var inputVal = Value.Create(shape, pVec, new BoolVector(), DeviceDescriptor.CPUDevice, false, false);
            return inputVal;
        }
    }

}

