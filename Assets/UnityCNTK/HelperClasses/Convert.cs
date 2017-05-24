using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using System;
using UnityEngine.Assertions;
namespace UnityCNTK
{
    public static class Convert
    {
        public static Value ToValue(this IEnumerable<Texture2D> textures, bool useAlpha = false)
        {
            Assert.AreNotEqual(textures.Count(), 0);
            List<float[]> list = new List<float[]>();
            int tensorWidth = textures.FirstOrDefault().width;
            int tensorHeight = textures.FirstOrDefault().height;
            NDShape shape = NDShape.CreateNDShape(new int[] { useAlpha ? 4 : 3, tensorHeight, tensorHeight });
            int channelPixelCount = tensorHeight * tensorWidth;
            foreach (var texture in textures)
            {
                Assert.AreEqual(texture.width, tensorWidth);
                Assert.AreEqual(texture.height, tensorHeight);
                var pixels = texture.GetPixels();
                var pixelArray = new float[channelPixelCount * (useAlpha ? 4 : 3)];
                Array.Copy(Array.ConvertAll(pixels, p => (float)p.r), 0, pixelArray, 0, channelPixelCount);
                Array.Copy(Array.ConvertAll(pixels, p => (float)p.g), 0, pixelArray, channelPixelCount, channelPixelCount);
                Array.Copy(Array.ConvertAll(pixels, p => (float)p.b), 0, pixelArray, channelPixelCount * 2, channelPixelCount);
                if (useAlpha) Array.Copy(Array.ConvertAll(pixels, p => (float)p.a), 0, pixelArray, channelPixelCount * 3, channelPixelCount);
            }
            var inputVal = CNTK.Value.Create(shape, list, new List<bool>(list.Count).Select(x => x = true), DeviceDescriptor.GPUDevice(0), false);
            return inputVal;
        }

        // Convert the main texturue of the material to texture
        public static Value ToValue(this IEnumerable<Material> mats, bool useAlpha = false)
        { 
            IEnumerable<Texture2D> textures = mats.Select(x => x.mainTexture as Texture2D);
            return textures.ToValue();
        } 

        // public static Value ToValue(this Vector3)
        // {

        // }
        // Summary:
        //     ///
        //     Create a new empty texture.
        //     ///
        //
        // Parameters:
        //   value: sadasda

        public static List<Texture2D> ToTexture2D(Value value, Variable variable)
        {
            List<Texture2D> texs = new List<Texture2D>();
            var dimemsions = value.Shape.Dimensions;
            var lists = value.GetDenseData<float>(variable);
            var rawTextures = from list in lists
                    select list.AsParallel().ToArray();
            List<Color[]> textureColor = new List<Color[]>(); 
            if(dimemsions[0] == 4)
            {

            }
            else if (dimemsions[0] == 3)
            {

            }
            else
            {
                throw new ApplicationException("the rawtexture should be RGB / ARGB");
            }
            for (int i = 0; i < rawTextures.Count(); i++)
            {
                Texture2D texture = new Texture2D(dimemsions[1], dimemsions[2]);
                  
            }
            return texs;
        }

        
    }

}

