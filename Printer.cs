using System;
using System.Globalization;
using System.IO;
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
		else if (type.EndsWith(']')) value = ReadArray(type[..^2]);
		else if (type == nameof(Boolean)) value = binaryReader.ReadBoolean().ToString();
		else if (type == nameof(Char)) value = binaryReader.ReadChar().ToString();
		else if (type == nameof(Byte)) value = binaryReader.ReadByte().ToString();
		else if (type == nameof(SByte)) value = binaryReader.ReadSByte().ToString();
		else if (type == nameof(Int16)) value = binaryReader.ReadInt16().ToString();
		else if (type == nameof(UInt16)) value = binaryReader.ReadUInt16().ToString();
		else if (type == nameof(Int32)) value = binaryReader.ReadInt32().ToString();
		else if (type == nameof(UInt32)) value = binaryReader.ReadUInt32().ToString();
		else if (type == nameof(Int64)) value = binaryReader.ReadInt64().ToString();
		else if (type == nameof(UInt64)) value = binaryReader.ReadUInt64().ToString();
		else if (type == nameof(Single)) value = binaryReader.ReadSingle().ToString(CultureInfo.InvariantCulture);
		else if (type == nameof(Double)) value = binaryReader.ReadDouble().ToString(CultureInfo.InvariantCulture);
		else if (type == nameof(Decimal)) value = binaryReader.ReadDecimal().ToString(CultureInfo.InvariantCulture);
		else if (type == nameof(String)) value = binaryReader.ReadString();
		else if (type == nameof(DateTime)) value = DateTime.FromBinary(binaryReader.ReadInt64()).ToString(CultureInfo.InvariantCulture);
		else if (type == nameof(TimeSpan)) value = TimeSpan.FromTicks(binaryReader.ReadInt64()).ToString();
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
			stringBuilder.Append(PrintValue('=', type));
			if (i != length - 1) stringBuilder.Append(", ");
		}

		stringBuilder.Append(']');
		if (binaryReader.ReadChar() != ']') throw new Exception();
		return stringBuilder.ToString();
	}
}