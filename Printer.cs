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
		string value = PrintValue(valueSign, type);
		return $"{type} {key} = {value};";
	}

	private string PrintValue(char valueSign, string type)
	{
		string value;
		if (valueSign == '~') value = "null";
		else if (type == typeof(bool).Name) value = binaryReader.ReadBoolean().ToString();
		else if (type == typeof(char).Name) value = binaryReader.ReadChar().ToString();
		else if (type == typeof(byte).Name) value = binaryReader.ReadByte().ToString();
		else if (type == typeof(sbyte).Name) value = binaryReader.ReadSByte().ToString();
		else if (type == typeof(short).Name) value = binaryReader.ReadInt16().ToString();
		else if (type == typeof(ushort).Name) value = binaryReader.ReadUInt16().ToString();
		else if (type == typeof(int).Name) value = binaryReader.ReadInt32().ToString();
		else if (type == typeof(uint).Name) value = binaryReader.ReadUInt32().ToString();
		else if (type == typeof(long).Name) value = binaryReader.ReadInt64().ToString();
		else if (type == typeof(ulong).Name) value = binaryReader.ReadUInt64().ToString();
		else if (type == typeof(float).Name) value = binaryReader.ReadSingle().ToString();
		else if (type == typeof(double).Name) value = binaryReader.ReadDouble().ToString();
		else if (type == typeof(decimal).Name) value = binaryReader.ReadDecimal().ToString();
		else if (type == typeof(string).Name) value = binaryReader.ReadString();
		else if (type == typeof(DateTime).Name) value = DateTime.FromBinary(binaryReader.ReadInt64()).ToString();
		else if (type == typeof(TimeSpan).Name) value = TimeSpan.FromTicks(binaryReader.ReadInt64()).ToString();
		else if (type.EndsWith(']')) value = ReadArray(type[..^2]);
		else value = ReadObject();
		if (binaryReader.ReadChar() != ';') throw new Exception();
		return value;
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

	private string ReadArray(string type)
	{
		int length = binaryReader.Read7BitEncodedInt();
		if (binaryReader.ReadChar() != '[') throw new Exception();
		StringBuilder stringBuilder = new("[");

		for (int i = 0; i < length; i++)
		{
			stringBuilder.Append(PrintValue('=',type));
			if (i != length - 1) stringBuilder.Append(", ");
		}

		stringBuilder.Append(']');
		if (binaryReader.ReadChar() != ']') throw new Exception();
		return stringBuilder.ToString();
	}
}