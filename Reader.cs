using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace BOX;

public class Reader(BinaryReader binaryReader) : IReader
{
	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		string typeName = binaryReader.ReadString();
		string expectedKey = binaryReader.ReadString();
		Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		key = key[(key.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"key mismatch '{type} {key}', expected '{typeName} {expectedKey}'");
		if (typeName != type.Name) Debug.WriteLine($"type mismatch '{type} {key}', expected '{typeName} {expectedKey}'");
		char valueSign = binaryReader.ReadChar();
		if (valueSign is not ('=' or '~')) throw new Exception();
		value = (T)ReadValue(valueSign, type);
	}

	public T Read<T>(T? _, [CallerArgumentExpression(nameof(_))] string key = "")
	{
		Read(out T? value, key);
		return value!;
	}

	private object ReadValue(char valueSign, Type type)
	{
		object? value;
		if (valueSign == '~') value = null;
		else if (type.IsAssignableTo(typeof(IBox))) value = ReadObject(type);
		else if (type == typeof(bool)) value = binaryReader.ReadBoolean();
		else if (type == typeof(char)) value = binaryReader.ReadChar();
		else if (type == typeof(byte)) value = binaryReader.ReadByte();
		else if (type == typeof(sbyte)) value = binaryReader.ReadSByte();
		else if (type == typeof(short)) value = binaryReader.ReadInt16();
		else if (type == typeof(ushort)) value = binaryReader.ReadUInt16();
		else if (type == typeof(int)) value = binaryReader.ReadInt32();
		else if (type == typeof(uint)) value = binaryReader.ReadUInt32();
		else if (type == typeof(long)) value = binaryReader.ReadInt64();
		else if (type == typeof(ulong)) value = binaryReader.ReadUInt64();
		else if (type == typeof(float)) value = binaryReader.ReadSingle();
		else if (type == typeof(double)) value = binaryReader.ReadDouble();
		else if (type == typeof(decimal)) value = binaryReader.ReadDecimal();
		else if (type == typeof(string)) value = binaryReader.ReadString();
		else if (type == typeof(DateTime)) value = DateTime.FromBinary(binaryReader.ReadInt64());
		else if (type == typeof(TimeSpan)) value = TimeSpan.FromTicks(binaryReader.ReadInt64());
		else if (type == typeof(char[])) value = ReadArray(binaryReader.ReadChars);
		else if (type.IsAssignableTo(typeof(byte[]))) value = ReadArray(binaryReader.ReadBytes);
		else if (type.IsArray) value = ReadArray(type.GetElementType()!);
		else throw new NotSupportedException(type.FullName);
		if (binaryReader.ReadChar() != ';') throw new Exception();
		return value!;
	}

	private object ReadObject(Type type)
	{
		if (binaryReader.ReadChar() != '{') throw new Exception();
		IBox box = (IBox)RuntimeHelpers.GetUninitializedObject(type);
		box.ReadFrom(this);
		if (binaryReader.ReadChar() != '}') throw new Exception();
		return box;
	}

	private Array ReadArray(Type type)
	{
		int length = binaryReader.Read7BitEncodedInt();
		if (binaryReader.ReadChar() != '[') throw new Exception();
		Array array = Array.CreateInstance(type, length);
		for (int i = 0; i < length; i++) array.SetValue(ReadValue('=', type), i);
		if (binaryReader.ReadChar() != ']') throw new Exception();
		return array;
	}

	private T ReadArray<T>(Func<int, T> func)
	{
		int length = binaryReader.Read7BitEncodedInt();
		if (binaryReader.ReadChar() != '"') throw new Exception();
		T array = func(length);
		if (binaryReader.ReadChar() != '"') throw new Exception();
		return array;
	}

	public static void Read<T>(out T value, byte[] bytes, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		using MemoryStream memoryStream = new(bytes);
		using BinaryReader binaryReader = new(memoryStream);
		Reader reader = new(binaryReader);
		reader.Read(out value, key);
	}

	public static T Read<T>(T? _, byte[] bytes, [CallerArgumentExpression(nameof(_))] string key = "")
	{
		Read(out T value, bytes, key);
		return value;
	}
}