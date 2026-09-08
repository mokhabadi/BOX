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

		switch (value)
		{
			case bool x: binaryWriter.Write(x); break;
			case char x: binaryWriter.Write(x); break;
			case byte x: binaryWriter.Write(x); break;
			case sbyte x: binaryWriter.Write(x); break;
			case short x: binaryWriter.Write(x); break;
			case ushort x: binaryWriter.Write(x); break;
			case int x: binaryWriter.Write(x); break;
			case uint x: binaryWriter.Write(x); break;
			case long x: binaryWriter.Write(x); break;
			case ulong x: binaryWriter.Write(x); break;
			case float x: binaryWriter.Write(x); break;
			case double x: binaryWriter.Write(x); break;
			case decimal x: binaryWriter.Write(x); break;
			case string x: binaryWriter.Write(x); break;
			case DateTime x: binaryWriter.Write(x.ToBinary()); break;
			case TimeSpan x: binaryWriter.Write(x.Ticks); break;
			case IBox x: WriteObject(x); break;
			case bool[] x: WriteArray(x, binaryWriter.Write); break;
			case char[] x: binaryWriter.Write(x); break;
			case byte[] x: binaryWriter.Write(x); break;
			case short[] x: WriteArray(x, binaryWriter.Write); break;
			case int[] x: WriteArray(x, binaryWriter.Write); break;
			case long[] x: WriteArray(x, binaryWriter.Write); break;
			case float[] x: WriteArray(x, binaryWriter.Write); break;
			case double[] x: WriteArray(x, binaryWriter.Write); break;
			case decimal[] x: WriteArray(x, binaryWriter.Write); break;
			case string[] x: WriteArray(x, binaryWriter.Write); break;
			case DateTime[] x: WriteArray(x, x => binaryWriter.Write(x.ToBinary())); break;
			case TimeSpan[] x: WriteArray(x, x => binaryWriter.Write(x.Ticks)); break;
			case IBox[] x: WriteArray(x, WriteObject); break;
			default: throw new NotSupportedException(value!.ToString());
		}
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