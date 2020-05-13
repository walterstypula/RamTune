using info.lundin.math;
using RamTune.Core.Metadata;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

public static class StreamExtensions
{
    public static byte[] ReadElement(this Stream stream, string endian, int byteArraySize)
    {
        var bytes = new byte[byteArraySize];
        stream.Read(bytes, 0, byteArraySize);
        bytes.ReverseBytes(endian);

        return bytes;
    }

    public static byte[] SeekAndReadElement(this Stream stream, string address, string endian, int byteArraySize)
    {
        stream.Seek(address.ConvertHexToInt(), SeekOrigin.Begin);
        return stream.ReadElement(endian, byteArraySize);
    }

    public static List<List<byte[]>> Read(this Stream stream, int address, int columnElements, int rowElements, string endian, int byteArraySize)
    {
        var tableData = new List<List<byte[]>>(rowElements);// columnElements][];

        stream.Seek(address, SeekOrigin.Begin);

        for (int row = 0; row < rowElements; row++)
        {
            var columns = new byte[columnElements][];
            for (int column = 0; column < columnElements; column++)
            {
                var byteValue = stream.ReadElement(endian, byteArraySize);
                columns[column] = byteValue;
            }

            tableData.Add(columns.ToList());
        }

        return tableData;
    }
}

public static class ByteExtensions
{
    public static void ReverseBytes(this byte[] inputBytes, string endian)
    {
        if (endian == null || endian?.ToLower() != "big")
        {
            return;
        }

        Array.Reverse(inputBytes);
    }
}

public static class StorageTypeExtensions
{
    public static int ParseStorageSize(this StorageType input)
    {
        switch (input)
        {
            case StorageType.bloblist:
                return 0;

            case StorageType.int8:
            case StorageType.uint8:
                return 1;

            case StorageType.int16:
            case StorageType.uint16:
                return 2;

            case StorageType.int32:
            case StorageType.uint32:
            case StorageType.@float:
                return 4;

            default:
                throw new Exception($"{input.ToString()} is a unhandled (unsupported) storage type.");
        }
    }

    public static int ParseStorageType(this StorageType input)
    {
        switch (input)
        {
            case StorageType.uint32:
            case StorageType.int32:
                return 4;

            case StorageType.uint16:
            case StorageType.int16:
                return 2;

            case StorageType.@float:
                return 99;

            case StorageType.uint8:
            case StorageType.int8:
                return 1;

            default:
                throw new Exception($"{input.ToString()} is a unhandled (unsupported) storage type.");
        }
    }
}

public static class ScalingExtensions
{
    public static byte[] GetBytesValue(this Scaling scaling, string data)
    {
        var storageType = scaling.StorageType;

        if (storageType == StorageType.bloblist)
        {
            var stringValue = scaling.Data.First(d => d.Name == data).Value;
            var byteValue = stringValue.ConvertHexIntoBytes();

            return byteValue;
        }

        var expression = scaling.FrExpr;
        var value = double.Parse(data);
        var output = value.CalcValue(expression);

        switch (storageType)
        {
            case StorageType.uint32:
                return BitConverter.GetBytes((uint)output);

            case StorageType.int32:
                return BitConverter.GetBytes((int)output);

            case StorageType.uint16:
                return BitConverter.GetBytes((ushort)output);

            case StorageType.int16:
                return BitConverter.GetBytes((short)output);

            case StorageType.@float:
                return BitConverter.GetBytes((float)output);

            case StorageType.uint8:
                return BitConverter.GetBytes((sbyte)output);

            case StorageType.int8:
                return BitConverter.GetBytes((byte)output);

            case StorageType.Unknown:
                break;

            default:
                break;
        }

        var bytesData = BitConverter.GetBytes(output);
        return bytesData;
    }

    public static string GetDisplayValue(this Scaling scaling, byte[] byteData)
    {
        var storageType = scaling.StorageType;
        double value;

        switch (storageType)
        {
            case StorageType.uint16:
                value = BitConverter.ToUInt16(byteData);
                break;

            case StorageType.int16:
                value = BitConverter.ToInt16(byteData);
                break;

            case StorageType.uint32:
                value = BitConverter.ToUInt32(byteData);
                break;

            case StorageType.int32:
                value = BitConverter.ToInt32(byteData);
                break;

            case StorageType.uint8:
                value = (byte)byteData[0];
                break;

            case StorageType.int8:
                value = (sbyte)byteData[0];
                break;

            case StorageType.@float:
                value = (double)BitConverter.ToSingle(byteData);
                break;

            case StorageType.bloblist:
                {
                    var scalingData = scaling.Data;
                    var data = BitConverter.ToString(byteData).Replace("-", string.Empty);
                    return scalingData.First(s => s.Value == data).Name;
                }

            default:
                throw new Exception($"{storageType.ToString()} is a unhandled (unsupported) storage type.");
        }

        var expression = scaling.ToExpr;
        var format = scaling.Format;

        return value.CalcValue(expression, format).ToString();
    }

    public static int ParseStorageSize(this Scaling scaling)
    {
        var storageType = scaling.StorageType;
        var byteArraySize = storageType.ParseStorageSize();

        if (!(byteArraySize == 0 && scaling?.Data is List<ScalingData> data))
        {
            return byteArraySize;
        }

        var result = data.GroupBy(d => d.Value.Length);

        if (result.Count() > 1)
        {
            throw new InvalidDataException($"{scaling.Name} bloblist length is not consistent.");
        }

        byteArraySize = result.First().Key / 2;

        return byteArraySize;
    }
}

public static class GlobalExtensions
{
    public static byte[] ConvertHexIntoBytes(this string hexInput)
    {
        return Enumerable.Range(0, hexInput.Length / 2)
                         .Select(x => Convert.ToByte(hexInput.Substring(x * 2, 2), 16))
                         .ToArray();
    }

    public static int ConvertHexToInt(this String hexInput)
    {
        return int.Parse(hexInput, NumberStyles.AllowHexSpecifier);
    }

    private static int Format(string format)
    {
        try
        {
            format = format.Replace("%.", string.Empty).Replace("%", string.Empty).Replace("f", string.Empty);
            return Convert.ToInt32(format);
        }
        catch
        {
            return 2;
        }
    }

    public static double CalcValue(this double value, string expression, string format = null)
    {
        ExpressionParser a = new ExpressionParser();
        a.Values.Add("x", value);
        var result = a.Parse(expression);

        if (format == null)
        {
            return result;
        }

        var precision = Format(format);
        return Math.Round((double)result, precision);
    }

    /// <summary>
    /// [ <c>public static object GetDefault(Type type)</c> ]
    /// <para></para>
    /// Retrieves the default value for a given Type
    /// </summary>
    /// <param name="type">The Type for which to get the default value</param>
    /// <returns>The default value for <paramref name="type"/></returns>
    /// <remarks>
    /// If a null Type, a reference Type, or a System.Void Type is supplied, this method always returns null.  If a value type
    /// is supplied which is not publicly visible or which contains generic parameters, this method will fail with an
    /// exception.
    /// </remarks>
    /// <seealso cref="GetDefault&lt;T&gt;"/>
    public static object GetDefault(this Type type)
    {
        // If no Type was supplied, if the Type was a reference type, or if the Type was a System.Void, return null
        if (type == null || !type.IsValueType || type == typeof(void))
            return null;

        // If the supplied Type has generic parameters, its default value cannot be determined
        if (type.ContainsGenericParameters)
            throw new ArgumentException(
                "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
                "> contains generic parameters, so the default value cannot be retrieved");

        // If the Type is a primitive type, or if it is another publicly-visible value type (i.e. struct), return a
        //  default instance of the value type
        if (type.IsPrimitive || !type.IsNotPublic)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch (Exception e)
            {
                throw new ArgumentException(
                    "{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe Activator.CreateInstance method could not " +
                    "create a default instance of the supplied value type <" + type +
                    "> (Inner Exception message: \"" + e.Message + "\")", e);
            }
        }

        // Fail with exception
        throw new ArgumentException("{" + MethodInfo.GetCurrentMethod() + "} Error:\n\nThe supplied value type <" + type +
            "> is not a publicly-visible type, so the default value cannot be retrieved");
    }
}