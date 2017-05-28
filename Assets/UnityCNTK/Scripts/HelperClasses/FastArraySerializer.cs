using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.InteropServices;

public static unsafe class FastArraySerializer
{
    [StructLayout(LayoutKind.Explicit)]
    private struct Union
    {
        [FieldOffset(0)] public byte[] bytes;
        [FieldOffset(0)] public float[] floats;
        [FieldOffset(0)] public sbyte[] int8s;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    private struct ArrayHeader
    {
        public UIntPtr type;
        public UIntPtr length;
    }

    private static readonly UIntPtr BYTE_ARRAY_TYPE;
    private static readonly UIntPtr FLOAT_ARRAY_TYPE;
    private static readonly UIntPtr INT8_ARRAY_TYPE;

    static FastArraySerializer()
    {
        fixed (void* pBytes = new byte[1])
        fixed (void* pFloats = new float[1])
        fixed (void* pInt8s = new sbyte[1])
        {
            BYTE_ARRAY_TYPE = getHeader(pBytes)->type;
            FLOAT_ARRAY_TYPE = getHeader(pFloats)->type;
            INT8_ARRAY_TYPE = getHeader(pInt8s)->type;
        }
    }

    public static void AsByteArray(this float[] floats, Action<byte[]> action)
    {
        if (floats.handleNullOrEmptyArray(action))
            return;

        var union = new Union { floats = floats };
        union.floats.toByteArray();
        try
        {
            action(union.bytes);
        }
        finally
        {
            union.bytes.toFloatArray();
        }
    }

    public static void AsFloatArray(this byte[] bytes, Action<float[]> action)
    {
        if (bytes.handleNullOrEmptyArray(action))
            return;

        var union = new Union { bytes = bytes };
        union.bytes.toFloatArray();
        try
        {
            action(union.floats);
        }
        finally
        {
            union.floats.toByteArray();
        }
    }

    public static void AsInt8Array(this byte[] bytes, Action<sbyte[]> action)
    {
        if (bytes.handleNullOrEmptyArray(action))
            return;

        var union = new Union { bytes = bytes };
        union.bytes.toFloatArray();
        try
        {
            action(union.int8s);
        }
        finally
        {
            union.int8s.toByteArray();
        }
    }


    public static bool handleNullOrEmptyArray<TSrc, TDst>(this TSrc[] array, Action<TDst[]> action)
    {
        if (array == null)
        {
            action(null);
            return true;
        }

        if (array.Length == 0)
        {
            action(new TDst[0]);
            return true;
        }

        return false;
    }

    private static ArrayHeader* getHeader(void* pBytes)
    {
        return (ArrayHeader*)pBytes - 1;
    }

    private static void toFloatArray(this byte[] bytes)
    {
        fixed (void* pArray = bytes)
        {
            var pHeader = getHeader(pArray);

            pHeader->type = FLOAT_ARRAY_TYPE;
            pHeader->length = (UIntPtr)(bytes.Length / sizeof(float));
        }
    }

    private static void toInt8Array(this byte[] bytes)
    {
        fixed (void* pArray = bytes)
        {
            var pHeader = getHeader(pArray);

            pHeader->type = INT8_ARRAY_TYPE;
            pHeader->length = (UIntPtr)(bytes.Length / sizeof(sbyte));
        }
    }

    private static void toByteArray(this sbyte[] int8s)
    {
        fixed (void* pArray = int8s)
        {
            var pHeader = getHeader(pArray);

            pHeader->type = BYTE_ARRAY_TYPE;
            pHeader->length = (UIntPtr)(int8s.Length * sizeof(sbyte));
        }
    }



    private static void toByteArray(this float[] floats)
    {
        fixed (void* pArray = floats)
        {
            var pHeader = getHeader(pArray);

            pHeader->type = BYTE_ARRAY_TYPE;
            pHeader->length = (UIntPtr)(floats.Length * sizeof(float));
        }
    }

    public static T[] Slice<T>(this T[] source, int index, int length)
    {
        T[] slice = new T[length];
        Array.Copy(source, index, slice, 0, length);
        return slice;
    }

    public static T[][] Slice<T>(this T[] source, int slices)
    {
        T[][] sliced = new T[slices][];
        var offset = source.Length % slices;
        var sliceSize = (source.Length - offset) / slices;
        for (int i = 0; i < slices; i++)
        {
            if (i != slices - 1)
            {
                sliced[i] = new T[sliceSize + 1];
                Array.Copy(source, i * sliceSize, sliced[i], 0, sliceSize);
            }
            else
            {
                sliced[i] = new T[sliceSize + offset + 1];
                Array.Copy(source, i * sliceSize, sliced[i], 0, sliceSize + offset);
            }

        }
        return sliced;
    }

}