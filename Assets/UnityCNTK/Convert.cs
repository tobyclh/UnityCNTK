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
            var pixels = texture.GetPixels();
            var channelPixelCount = texture.width * texture.height;
            var pixelArray = new float[channelPixelCount * (useAlpha ? 4 : 3)];
            Array.Copy(Array.ConvertAll(pixels, p => (float)p.r), 0, pixelArray, 0, channelPixelCount);
            Array.Copy(Array.ConvertAll(pixels, p => (float)p.g), 0, pixelArray, channelPixelCount, channelPixelCount);
            Array.Copy(Array.ConvertAll(pixels, p => (float)p.b), 0, pixelArray, channelPixelCount * 2, channelPixelCount);
            if (useAlpha) Array.Copy(Array.ConvertAll(pixels, p => (float)p.a), 0, pixelArray, channelPixelCount * 3, channelPixelCount);
            List<float[]> list = new List<float[]>() { pixelArray };
            var inputVal = CNTK.Value.Create(shape, list, new bool[] { true }, DeviceDescriptor.GPUDevice(0), false);
            return inputVal;
        }

        public static CNTK.Value ToValue(Material mat)
        {
            return Convert.ToValue(mat.mainTexture as Texture2D);
        }
        // Summary:
        //     ///
        //     Create a new empty texture.
        //     ///
        //
        // Parameters:
        //   value: sadasda

        public static List<Texture2D> ToTexture2D(this Value value)
        {
            List<Texture2D> texs = new List<Texture2D>();
            var dimemsions = value.Shape.Dimensions;
            for(int i = 0; i < dimemsions[3]; i++)
            {
                Texture2D texture = new Texture2D(dimemsions[1], dimemsions[2]);

            }
            return texs;
        }
    }

}

