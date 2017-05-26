using UnityEngine;
using CNTK;
using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace UnityCNTK
{
    //Simple 
    public static class Convert
    {
        public static Value ToValue(this IEnumerable<Texture2D> textures, DeviceDescriptor device)
        {
            bool useAlpha = textures.First().GetPixel(0, 0).a == 0 ? true : false;
            Assert.AreNotEqual(textures.Count(), 0);
            List<float> list = new List<float>();
            int tensorWidth = textures.FirstOrDefault().width;
            int tensorHeight = textures.FirstOrDefault().height;
            NDShape shape = NDShape.CreateNDShape(new int[] { tensorWidth, tensorHeight, useAlpha ? 4 : 3 });
            int channelPixelCount = tensorHeight * tensorWidth;
            Parallel.For(0, textures.Count(), (int batchNum) =>
           {
               Assert.AreEqual(textures.ElementAt(batchNum).width, tensorWidth);
               Assert.AreEqual(textures.ElementAt(batchNum).height, tensorHeight);
               var pixels = textures.ElementAt(batchNum).GetPixels();
               var pixelArray = new float[tensorHeight * tensorWidth * (useAlpha ? 4 : 3)];
               Parallel.For(0, (useAlpha ? 4 : 3) - 1, (int c) =>
                 {
                     for (int i = 0; i < tensorHeight * tensorWidth; i++)
                     {
                         pixelArray[i + c] = pixels[i][c];
                     }
                 });
               list.AddRange(pixelArray);
           });
            return Value.CreateBatch(shape, list, device, false);
        }



        // Convert the main texturue of the material to texture
        public static Value ToValue(this IEnumerable<Material> mats, DeviceDescriptor device)
        {
            IEnumerable<Texture2D> textures = mats.Select(x => x.mainTexture as Texture2D);
            return textures.ToValue(device);
        }


        public static Value ToValue(this IEnumerable<Vector3> vector, DeviceDescriptor device)
        {
            Assert.AreNotEqual(vector.Count(), 0);
            float[] floatArray = new float[vector.Count()];
            NDShape shape = NDShape.CreateNDShape(new int[] { 3, 1, 1 });
            Parallel.For(0, vector.Count(), (int batchNum) =>
            {
                vector[batchNum*3] = vector[batchNum].x;
            });
            return Value.CreateBatch(shape, list, device, false);

        }
        public static List<Texture2D> ToTexture2D(Value value, Variable variable)
        {
            List<Texture2D> texs = new List<Texture2D>();
            var dimemsions = value.Shape.Dimensions;
            var lists = value.GetDenseData<float>(variable);
            var rawTextures = from list in lists.AsParallel()
                              select list.ToArray();
            if (value.Shape.TotalSize % dimemsions[0] * dimemsions[1] * dimemsions[2] != 0) throw new ApplicationException("Size unmatch");
            else if (dimemsions[2] != 3 && dimemsions[2] != 4) throw new ApplicationException("Image must be 3 / 4 dimension");
            Parallel.For(0, rawTextures.Count(), (int t) =>
            {
                Color[] pixels = new Color[dimemsions[0] * dimemsions[1] * dimemsions[2]];
                pixels.Initialize();
                Parallel.For(0, dimemsions[2], (int c) =>
                {
                    for (int i = 0; i < dimemsions[0] * dimemsions[1]; i++)
                    {
                        pixels[i][c] = rawTextures.ElementAt(t)[i + c];
                    }
                });

            });
            return texs;
        }



        public static Texture2D ResampleAndCrop(this Texture2D source, int targetWidth, int targetHeight)
        {
            int sourceWidth = source.width;
            int sourceHeight = source.height;
            float sourceAspect = (float)sourceWidth / sourceHeight;
            float targetAspect = (float)targetWidth / targetHeight;
            int xOffset = 0;
            int yOffset = 0;
            float factor = 1;
            if (sourceAspect > targetAspect)
            { // crop width
                factor = (float)targetHeight / sourceHeight;
                xOffset = (int)((sourceWidth - sourceHeight * targetAspect) * 0.5f);
            }
            else
            { // crop height
                factor = (float)targetWidth / sourceWidth;
                yOffset = (int)((sourceHeight - sourceWidth / targetAspect) * 0.5f);
            }
            Color32[] data = source.GetPixels32();
            Color32[] data2 = new Color32[targetWidth * targetHeight];
            for (int y = 0; y < targetHeight; y++)
            {
                for (int x = 0; x < targetWidth; x++)
                {
                    var p = new Vector2(Mathf.Clamp(xOffset + x / factor, 0, sourceWidth - 1), Mathf.Clamp(yOffset + y / factor, 0, sourceHeight - 1));
                    // bilinear filtering
                    var c11 = data[Mathf.FloorToInt(p.x) + sourceWidth * (Mathf.FloorToInt(p.y))];
                    var c12 = data[Mathf.FloorToInt(p.x) + sourceWidth * (Mathf.CeilToInt(p.y))];
                    var c21 = data[Mathf.CeilToInt(p.x) + sourceWidth * (Mathf.FloorToInt(p.y))];
                    var c22 = data[Mathf.CeilToInt(p.x) + sourceWidth * (Mathf.CeilToInt(p.y))];
                    var f = new Vector2(Mathf.Repeat(p.x, 1f), Mathf.Repeat(p.y, 1f));
                    data2[x + y * targetWidth] = Color.Lerp(Color.Lerp(c11, c12, p.y), Color.Lerp(c21, c22, p.y), p.x);
                }
            }

            var tex = new Texture2D(targetWidth, targetHeight);
            tex.SetPixels32(data2);
            tex.Apply(true);
            return tex;
        }

    }

}

