using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace BOX;

public class Writer(BinaryWriter binaryWriter) : IWriter
{
	public void Write<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		binaryWriter.Write(type.Name);
		binaryWriter.Write(key);
		binaryWriter.Write(value != null ? '=' : '~');
		WriteValue(value);
	}

	private void WriteValue<T>(T? value)
	{
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
		else if (value is char[] chars) WriteArray(chars, binaryWriter.Write);
		else if (value is byte[] bytes) WriteArray(bytes, binaryWriter.Write);
		else if (value is Array array) WriteArray(array);
		else if (value != null) throw new NotSupportedException(typeof(T).FullName);
		binaryWriter.Write(';');
	}

	private void WriteObject(IBox box)
	{
		binaryWriter.Write('{');
		box.WriteTo(this);
		binaryWriter.Write('}');
	}

	private void WriteArray(Array array)
	{
		if(array.Rank != 1) throw new NotSupportedException();
		binaryWriter.Write7BitEncodedInt(array.Length);
		binaryWriter.Write('[');
		for (int i = 0; i < array.Length; i++) WriteValue(array.GetValue(i));
		binaryWriter.Write(']');
	}

	private void WriteArray<T>(T[] array, Action<T[]> action)
	{
		binaryWriter.Write7BitEncodedInt(array.Length);
		binaryWriter.Write('"');
		action(array);
		binaryWriter.Write('"');
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