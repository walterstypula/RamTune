using info.lundin.math;
using RamTune.Core.Metadata;
using System;
using System.Reflection;

public static class GlobalExtensions
{
    public static int ConvertHexToInt(this String hexInput)
    {
        return int.Parse(hexInput, System.Globalization.NumberStyles.AllowHexSpecifier);
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

    public static string CalcValue(string expression, string format, double value)
    {
        ExpressionParser a = new ExpressionParser();
        a.Values.Add("x", value);
        var result = a.Parse(expression);
        var precision = Format(format);
        return Math.Round((decimal)result, precision).ToString();
    }

    public static string ParseDataValue(this byte[] byteData, StorageType storageType, string expression, string format)
    {
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
                return BitConverter.ToString(byteData).Replace("-", string.Empty);

            default:
                throw new Exception($"{storageType.ToString()} is a unhandled (unsupported) storage type.");
        }

        return CalcValue(expression, format, value).ToString();
    }

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

    public static void ReverseBytes(this byte[] inputBytes, string endian)
    {
        if (endian == null || endian?.ToLower() != "big")
        {
            return;
        }

        Array.Reverse(inputBytes);
    }

    public static T Cast<T>(object o)
    {
        return (T)o;
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