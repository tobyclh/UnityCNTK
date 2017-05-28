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
        /// <summary>
        /// 
        /// </summary>
        /// <param name="textures"></param>
        /// <param name="device">which device should this value go</param>
        /// <param name="mono">should we turn this into a monochrome picture</param>
        /// <param name="smallTexture"></param>
        /// <returns></returns>
        public static Value ToValue(this IEnumerable<Texture2D> textures, DeviceDescriptor device, bool mono = false, bool smallTexture = true)
        {
            int count = textures.Count();
            Assert.AreNotEqual(count, 0);
            int tensorWidth = textures.FirstOrDefault().width;
            int tensorHeight = textures.FirstOrDefault().height;
            int channelSize = tensorHeight * tensorWidth;
            if (!mono)
            {
                int channelCount = textures.First().GetPixel(0, 0).a == 0 ? 3 : 4;
                int imageSize = channelSize * channelCount;
                float[] floatArray = new float[count * imageSize];
                NDShape shape = NDShape.CreateNDShape(new int[] { tensorWidth, tensorHeight, channelCount });
                Parallel.For(0, count, (int imageCounter) =>
                {

                    var texture = textures.ElementAt(imageCounter);
                    
                    Assert.AreEqual(texture.width, tensorWidth);
                    Assert.AreEqual(texture.height, tensorHeight);
                    var pixels = texture.GetPixels();
                    int pixelCount = pixels.Count();
                    Parallel.For(0, channelCount, (int c) =>
                    {
                        for (int i = 0; i < pixelCount; i++)
                        {
                            floatArray[imageCounter * imageSize + i * channelCount + c] = pixels[pixelCount][c];
                        }
                    });
                });
                return Value.CreateBatch(shape, floatArray, device, false);
            }
            else
            {
                int imageSize = channelSize;
                float[] floatArray = new float[count * imageSize];
                NDShape shape = NDShape.CreateNDShape(new int[] { tensorWidth, tensorHeight, 1 });
                Parallel.For(0, count, (int imageCounter) =>
                {
                    var texture = textures.ElementAt(imageCounter);
                    Assert.AreEqual(texture.width, tensorWidth);
                    Assert.AreEqual(texture.height, tensorHeight);
                    var pixels = texture.GetPixels();
                    int pixelCount = pixels.Count();
                    for (int i = 0; i < pixelCount; i++)
                    {
                        floatArray[imageCounter * imageSize + i] = pixels[pixelCount].grayscale;
                    }
                });
                return Value.CreateBatch(shape, floatArray, device, false);

            }
        }

        // Convert the main texturue of the material to texture
        public static Value ToValue(this IEnumerable<Material> mats, DeviceDescriptor device)
        {
            IEnumerable<Texture2D> textures = mats.Select(x => x.mainTexture as Texture2D);
            return textures.ToValue(device);
        }


        public static Value ToValue(this IEnumerable<Vector3> vectors, DeviceDescriptor device)
        {
            var count = vectors.Count();
            Assert.AreNotEqual(count, 0, "Empty list");
            float[] floatArray = new float[count * 3];
            NDShape shape = NDShape.CreateNDShape(new int[] { 3 });
            Parallel.For(0, count, (int batchNum) =>
            {
                var vector = vectors.ElementAt(batchNum);
                floatArray[batchNum * 3] = vector.x;
                floatArray[batchNum * 3 + 1] = vector.y;
                floatArray[batchNum * 3 + 2] = vector.z;
            });
            return Value.CreateBatch(shape, floatArray, device, false);
        }

        public static Value ToValue(this IEnumerable<Quaternion> quats, DeviceDescriptor device)
        {
            Assert.AreNotEqual(quats.Count(), 0);
            float[] floatArray = new float[quats.Count()];
            NDShape shape = NDShape.CreateNDShape(new int[] { 4 });
            int count = quats.Count();
            Parallel.For(0, count, (int batchNum) =>
            {
                var quat = quats.ElementAt(batchNum);
                floatArray[batchNum * 4] = quat.w;
                floatArray[batchNum * 4 + 1] = quat.x;
                floatArray[batchNum * 4 + 2] = quat.y;
                floatArray[batchNum * 4 + 3] = quat.z;
            });
            return Value.CreateBatch(shape, floatArray, device, false);
        }

        public static List<Texture2D> ToTexture2D(this Value value, Variable variable)
        {
            List<Texture2D> texs = new List<Texture2D>();
            var dimemsions = value.Shape.Dimensions;
            var lists = value.GetDenseData<float>(variable);
            var rawTextures = lists.AsParallel();

            if ((value.Shape.TotalSize % dimemsions[0] * dimemsions[1] * dimemsions[2]) != 0) throw new ApplicationException("Size unmatch");
            else if (dimemsions[2] != 3 && dimemsions[2] != 4) throw new ApplicationException("Image must have 3 or 4 channels");
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




    }

}

