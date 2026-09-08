using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace Box;

public class Printer(BinaryReader binaryReader)
{
	public string Print()
	{
		StringBuilder result = new();

		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			string[] lines = PrintItem().Split(Environment.NewLine);
			int indent = 0;

			foreach (string line in lines)
			{
				if (line.StartsWith('}')) indent--;
				result.Append(' ', indent * 3);
				if (line.StartsWith('{')) indent++;
				result.AppendLine(line);
			}
		}

		return result.ToString();
	}

	private string PrintItem()
	{
		string key = binaryReader.ReadString();
		string type = binaryReader.ReadString();
		char valueSign = binaryReader.ReadChar();
		if (valueSign is not ('=' or '~')) throw new Exception();
		string value = valueSign == '=' ? PrintValue(type) : "null";
		if (binaryReader.ReadChar() != ';') throw new Exception();
		return $"{type} {key} = {value};";
	}

	private string PrintValue(string type)
	{
		int length = type.EndsWith(']') ? binaryReader.Read7BitEncodedInt() : 0;

		return type switch
		{
			nameof(Boolean) => binaryReader.ReadBoolean().ToString(),
			nameof(Char) => binaryReader.ReadChar().ToString(),
			nameof(Byte) => binaryReader.ReadByte().ToString(),
			nameof(SByte) => binaryReader.ReadSByte().ToString(),
			nameof(Int16) => binaryReader.ReadInt16().ToString(),
			nameof(UInt16) => binaryReader.ReadUInt16().ToString(),
			nameof(Int32) => binaryReader.ReadInt32().ToString(),
			nameof(UInt32) => binaryReader.ReadUInt32().ToString(),
			nameof(Int64) => binaryReader.ReadInt64().ToString(),
			nameof(UInt64) => binaryReader.ReadUInt64().ToString(),
			nameof(Single) => binaryReader.ReadSingle().ToString(CultureInfo.InvariantCulture),
			nameof(Double) => binaryReader.ReadDouble().ToString(CultureInfo.InvariantCulture),
			nameof(Decimal) => binaryReader.ReadDecimal().ToString(CultureInfo.InvariantCulture),
			nameof(String) => binaryReader.ReadString(),
			nameof(DateTime) => DateTime.FromBinary(binaryReader.ReadInt64()).ToString(CultureInfo.InvariantCulture),
			nameof(TimeSpan) => TimeSpan.FromTicks(binaryReader.ReadInt64()).ToString(),
			"Boolean[]" => ReadArray(length, binaryReader.ReadBoolean),
			"Char[]" => string.Join(',', binaryReader.ReadChars(length)),
			"Byte[]" or "SByte[]" => string.Join(',', binaryReader.ReadBytes(length)),
			"Int16[]" or "UInt16[]" => ReadArray(length, binaryReader.ReadInt16),
			"Int32[]" or "UInt32[]" => ReadArray(length, binaryReader.ReadInt32),
			"Int64[]" or "UInt64[]" => ReadArray(length, binaryReader.ReadInt64),
			"Single[]" => ReadArray(length, binaryReader.ReadSingle),
			"Double[]" => ReadArray(length, binaryReader.ReadDouble),
			"Decimal[]" => ReadArray(length, binaryReader.ReadDecimal),
			"String[]" => ReadArray(length, binaryReader.ReadString),
			"DateTime[]" => ReadArray(length, () => DateTime.FromBinary(binaryReader.ReadInt64())),
			"TimeSpan[]" => ReadArray(length, () => TimeSpan.FromTicks(binaryReader.ReadInt64())),
			_ when length > 0 => ReadArray(length),
			_ => ReadObject(),
		};
	}

	private string ReadArray<T>(int length, Func<T> func)
	{
		if (binaryReader.ReadChar() != '[') throw new Exception();
		StringBuilder stringBuilder = new("[");

		for (int i = 0; i < length; i++)
		{
			if (binaryReader.ReadChar() != ',') throw new Exception();
			stringBuilder.Append(func());
			if (i != length - 1) stringBuilder.Append(", ");
		}

		stringBuilder.Append(']');
		if (binaryReader.ReadChar() != ']') throw new Exception();
		return stringBuilder.ToString();
	}

	private string ReadArray(int length)
	{
		if (binaryReader.ReadChar() != '[') throw new Exception();
		StringBuilder stringBuilder = new("[");
		for (int i = 0; i < length; i++)
		{
			if (binaryReader.ReadChar() != ',') throw new Exception();
			stringBuilder.Append(ReadObject());
			if (i != length - 1) stringBuilder.Append(", ");
		}

		stringBuilder.Append(']');
		if (binaryReader.ReadChar() != ']') throw new Exception();
		return stringBuilder.ToString();
	}

	private string ReadObject()
	{
		StringBuilder stringBuilder = new();
		stringBuilder.AppendLine();
		stringBuilder.AppendLine("{");
		if (binaryReader.ReadChar() != '{') throw new Exception();
		while (binaryReader.PeekChar() != '}') stringBuilder.AppendLine(PrintItem());
		binaryReader.ReadChar();
		stringBuilder.Append('}');
		return stringBuilder.ToString();
	}
}