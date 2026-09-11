using System;
using System.Globalization;
using System.IO;
using System.Text;

namespace BOX;

public class Printer(BinaryReader binaryReader)
{
	public string Print()
	{
		StringBuilder stringBuilder = new();
		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length) PrintItem(stringBuilder);
		return stringBuilder.ToString();
	}

	private void PrintItem(StringBuilder stringBuilder)
	{
		string type = binaryReader.ReadString();
		string key = binaryReader.ReadString();
		stringBuilder.Append($"{type} {key} = ");
		char valueSign = binaryReader.ReadChar();
		PrintValue(valueSign, type, stringBuilder);
	}

	private void PrintValue(char valueSign, string type, StringBuilder stringBuilder)
	{
		if (valueSign == '~') stringBuilder.Append("null");
		else if (type == nameof(Boolean)) stringBuilder.Append(binaryReader.ReadBoolean().ToString());
		else if (type == nameof(Char)) stringBuilder.Append(binaryReader.ReadChar().ToString());
		else if (type == nameof(Byte)) stringBuilder.Append(binaryReader.ReadByte().ToString());
		else if (type == nameof(SByte)) stringBuilder.Append(binaryReader.ReadSByte().ToString());
		else if (type == nameof(Int16)) stringBuilder.Append(binaryReader.ReadInt16().ToString());
		else if (type == nameof(UInt16)) stringBuilder.Append(binaryReader.ReadUInt16().ToString());
		else if (type == nameof(Int32)) stringBuilder.Append(binaryReader.ReadInt32().ToString());
		else if (type == nameof(UInt32)) stringBuilder.Append(binaryReader.ReadUInt32().ToString());
		else if (type == nameof(Int64)) stringBuilder.Append(binaryReader.ReadInt64().ToString());
		else if (type == nameof(UInt64)) stringBuilder.Append(binaryReader.ReadUInt64().ToString());
		else if (type == nameof(Single)) stringBuilder.Append(binaryReader.ReadSingle().ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(Double)) stringBuilder.Append(binaryReader.ReadDouble().ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(Decimal)) stringBuilder.Append(binaryReader.ReadDecimal().ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(String)) stringBuilder.Append(binaryReader.ReadString());
		else if (type == nameof(DateTime)) stringBuilder.Append(DateTime.FromBinary(binaryReader.ReadInt64()).ToString(CultureInfo.InvariantCulture));
		else if (type == nameof(TimeSpan)) stringBuilder.Append(TimeSpan.FromTicks(binaryReader.ReadInt64()).ToString());
		else if (type == typeof(char[]).Name) ReadArray(binaryReader.ReadChars, stringBuilder);
		else if (type == typeof(byte[]).Name || type == typeof(sbyte[]).Name) ReadArray(binaryReader.ReadBytes, stringBuilder);
		else if (type.EndsWith("[]")) ReadArray(type[..^2], stringBuilder);
		else if (binaryReader.PeekChar() == '{') ReadObject(stringBuilder);
		else stringBuilder.Append("new");
		stringBuilder.Append(binaryReader.ReadChar()+ " ");
	}

	private void ReadObject(StringBuilder stringBuilder)
	{
		stringBuilder.Append(binaryReader.ReadChar());
		while (binaryReader.PeekChar() != '}') PrintItem(stringBuilder);
		stringBuilder.Append(binaryReader.ReadChar());
	}

	private void ReadArray(string type, StringBuilder stringBuilder)
	{
		int length = binaryReader.Read7BitEncodedInt();
		stringBuilder.Append(binaryReader.ReadChar());
		for (int i = 0; i < length; i++) PrintValue('=', type, stringBuilder);
		stringBuilder.Append(binaryReader.ReadChar());
	}

	private void ReadArray<T>(Func<int, T[]> func, StringBuilder stringBuilder)
	{
		int length = binaryReader.Read7BitEncodedInt();
		stringBuilder.Append(binaryReader.ReadChar());
		stringBuilder.Append(string.Join(" ", func(length)));
		stringBuilder.Append(binaryReader.ReadChar());
	}

	public static string Print(byte[] bytes)
	{
		using MemoryStream memoryStream = new(bytes);
		using BinaryReader binaryReader = new(memoryStream);
		Printer printer = new(binaryReader);
		return printer.Print();
	}
}