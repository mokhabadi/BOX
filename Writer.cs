using System;
using System.IO;
using System.Runtime.CompilerServices;
using Utility;

namespace Box;

public class Writer(BinaryWriter binaryWriter) : IWriter
{
	public void Write<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		binaryWriter.Write('|');
		binaryWriter.Write(key);
		binaryWriter.Write(value == null ? "null" : (Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T)).Name);
		if (value == null) return;
		if(value is Array array) binaryWriter.Write7BitEncodedInt(array.Length);
		
		Action action = value switch
		{
			bool x => () => binaryWriter.Write(x),
			char x => () => binaryWriter.Write(x),
			byte x => () => binaryWriter.Write(x),
			sbyte x => () => binaryWriter.Write(x),
			short x => () => binaryWriter.Write(x),
			ushort x => () => binaryWriter.Write(x),
			int x => () => binaryWriter.Write(x),
			uint x => () => binaryWriter.Write(x),
			long x => () => binaryWriter.Write(x),
			ulong x => () => binaryWriter.Write(x),
			float x => () => binaryWriter.Write(x),
			double x => () => binaryWriter.Write(x),
			decimal x => () => binaryWriter.Write(x),
			string x => () => binaryWriter.Write(x),
			DateTime x => () => binaryWriter.Write(x.ToBinary()),
			TimeSpan x => () => binaryWriter.Write(x.Ticks),
			IBox x => () => WriteObject(x),
			bool[] x => () => WriteArray(x, binaryWriter.Write),
			char[] x => () => binaryWriter.Write(x),
			byte[] x => () => binaryWriter.Write(x),
			sbyte[] x => () => WriteArray(x, binaryWriter.Write),
			short[] x => () => WriteArray(x, binaryWriter.Write),
			ushort[] x => () => WriteArray(x, binaryWriter.Write),
			int[] x => () => WriteArray(x, binaryWriter.Write),
			uint[] x => () => WriteArray(x, binaryWriter.Write),
			long[] x => () => WriteArray(x, binaryWriter.Write),
			ulong[] x => () => WriteArray(x, binaryWriter.Write),
			float[] x => () => WriteArray(x, binaryWriter.Write),
			double[] x => () => WriteArray(x, binaryWriter.Write),
			decimal[] x => () => WriteArray(x, binaryWriter.Write),
			string[] x => () => WriteArray(x, binaryWriter.Write),
			DateTime[] x => () => WriteArray(x, x => binaryWriter.Write(x.ToBinary())),
			TimeSpan[] x => () => WriteArray(x, x => binaryWriter.Write(x.Ticks)),
			IBox[] x => () => WriteArray(x, WriteObject),
			_ => throw new NotSupportedException(typeof(T).Name)
		};

		action();
	}

	void WriteObject(IBox box)
	{
		binaryWriter.Write('{');
		box.WriteTo(this);
		binaryWriter.Write('}');
	}

	void WriteArray<T>(T[] array, Action<T> action)
	{
		foreach (T t in array) action(t);
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