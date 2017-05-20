using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System;
namespace UnityCNTK
{
    public static class Convert
    {
        public static CNTK.Value ToValue(Texture2D texture, bool useAlpha = false)
        {
            var shape = NDShape.CreateNDShape(new int[] { useAlpha ? 4 : 3, texture.width, texture.height });
            var pixels = texture.GetRawTextureData();
            NDArrayView arrayView;
            //Non-copying way to create a value
            pixels.AsFloatArray(x => arrayView = new NDArrayView(shape, x, DeviceDescriptor.CPUDevice, false) );
            
            // var array = new float[useAlpha ? 4 : 3, texture.width * texture.height];
            
            NDArrayViewPtrVector pVec = new NDArrayViewPtrVector(pixels);

            // NDArrayView arrayView = new NDArrayView()

            var inputVal = CNTK.Value.Create(shape, pVec, new BoolVector() { true }, DeviceDescriptor.CPUDevice, false, false);
            return inputVal;
        }

        public static CNTK.Value ToValue(Material mat)
        {
            return Convert.ToValue(mat.mainTexture as Texture2D);
        }

    }

}

