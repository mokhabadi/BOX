using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Writer(BinaryWriter binaryWriter) : IWriter
{
	public void Write<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		binaryWriter.Write(key);
		Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		binaryWriter.Write(type.Name);
		binaryWriter.Write(value != null ? '=' : '~');
		if (value != null) WriteValue(value);
		binaryWriter.Write(';');
	}

	private void WriteValue<T>(T value)
	{
		if (value is Array array) binaryWriter.Write7BitEncodedInt(array.Length);
		if (value is IBox box) WriteObject(box);
		else if (value is bool @bool) binaryWriter.Write(@bool);
		else if (value is char @char) binaryWriter.Write(@char);
		else if (value is byte @byte) binaryWriter.Write(@byte);
		else if (value is sbyte @sbyte) binaryWriter.Write(@sbyte);
		else if (value is short @short) binaryWriter.Write(@short);
		else if (value is ushort @ushort) binaryWriter.Write(@ushort);
		else if (value is int @int) binaryWriter.Write(@int);
		else if (value is uint @uint) binaryWriter.Write(@uint);
		else if (value is long @long) binaryWriter.Write(@long);
		else if (value is ulong @ulong) binaryWriter.Write(@ulong);
		else if (value is float @float) binaryWriter.Write(@float);
		else if (value is double @double) binaryWriter.Write(@double);
		else if (value is decimal @decimal) binaryWriter.Write(@decimal);
		else if (value is string @string) binaryWriter.Write(@string);
		else if (value is DateTime dateTime) binaryWriter.Write(dateTime.ToBinary());
		else if (value is TimeSpan timeSpan) binaryWriter.Write(timeSpan.Ticks);
		else if (value is IBox[] boxArray) WriteArray(boxArray, WriteObject);
		else if (value is bool[] boolArray) WriteArray(boolArray, binaryWriter.Write);
		else if (value is char[] charArray) binaryWriter.Write(charArray);
		else if (value is byte[] byteArray) binaryWriter.Write(byteArray);
		else if (value is short[] shortArray) WriteArray(shortArray, binaryWriter.Write);
		else if (value is int[] intArray) WriteArray(intArray, binaryWriter.Write);
		else if (value is long[] longArray) WriteArray(longArray, binaryWriter.Write);
		else if (value is float[] floatArray) WriteArray(floatArray, binaryWriter.Write);
		else if (value is double[] doubleArray) WriteArray(doubleArray, binaryWriter.Write);
		else if (value is decimal[] decimalArray) WriteArray(decimalArray, binaryWriter.Write);
		else if (value is string[] stringArray) WriteArray(stringArray, binaryWriter.Write);
		else if (value is DateTime[] dateTimeArray) WriteArray(dateTimeArray, dateTime => binaryWriter.Write(dateTime.ToBinary()));
		else if (value is TimeSpan[] timeSpanArray) WriteArray(timeSpanArray, timeSpan => binaryWriter.Write(timeSpan.Ticks));
		else throw new NotSupportedException(value!.ToString());
	}

	private void WriteObject(IBox box)
	{
		binaryWriter.Write('{');
		box.WriteTo(this);
		binaryWriter.Write('}');
	}

	private void WriteArray<T>(T[] array, Action<T> action)
	{
		binaryWriter.Write('[');

		foreach (T t in array)
		{
			binaryWriter.Write(',');
			action(t);
		}

		binaryWriter.Write(']');
	}

	public static byte[] WriteToByteArray(IBox box)
	{
		using MemoryStream memoryStream = new();
		using BinaryWriter binaryWriter = new(memoryStream);
		Writer writer = new(binaryWriter);
		box.WriteTo(writer);
		byte[] bytes = memoryStream.ToArray();
		return bytes;
	}
}