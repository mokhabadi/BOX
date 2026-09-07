using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Reader(BinaryReader binaryReader) : IReader
{
	public void Read<T>(out T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		value = Read(default(T), key);
	}

	public T? Read<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		if (binaryReader.ReadChar() != '|') throw new Exception();
		string expectedKey = binaryReader.ReadString();
		key = key[(key.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"value mismatch '{key}', expected '{expectedKey}'");
		string typeName = binaryReader.ReadString();
		Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		if (typeName != type.Name) throw new Exception($"type mismatch '{type.Name}', expected '{typeName}'");
		char valueSign =  binaryReader.ReadChar();
		if(valueSign != '=' && valueSign != '~') throw new Exception();
		if(valueSign == '~') return default;
		int length = type.IsArray ? binaryReader.Read7BitEncodedInt() : 0;

		object @object = typeName switch
		{
			nameof(Boolean) => binaryReader.ReadBoolean(),
			nameof(Char) => binaryReader.ReadChar(),
			nameof(Byte) => binaryReader.ReadByte(),
			nameof(SByte) => binaryReader.ReadSByte(),
			nameof(Int16) => binaryReader.ReadInt16(),
			nameof(UInt16) => binaryReader.ReadUInt16(),
			nameof(Int32) => binaryReader.ReadInt32(),
			nameof(UInt32) => binaryReader.ReadUInt32(),
			nameof(Int64) => binaryReader.ReadInt64(),
			nameof(UInt64) => binaryReader.ReadUInt64(),
			nameof(Single) => binaryReader.ReadSingle(),
			nameof(Double) => binaryReader.ReadDouble(),
			nameof(Decimal) => binaryReader.ReadDecimal(),
			nameof(String) => binaryReader.ReadString(),
			nameof(DateTime) => DateTime.FromBinary(binaryReader.ReadInt64()),
			nameof(TimeSpan) => TimeSpan.FromTicks(binaryReader.ReadInt64()),
			"Boolean[]" => ReadArray(length, binaryReader.ReadBoolean),
			"Char[]" => binaryReader.ReadChars(length),
			"Byte[]" => binaryReader.ReadBytes(length),
			"SByte[]" => ReadArray(length, binaryReader.ReadSByte),
			"Int16[]" => ReadArray(length, binaryReader.ReadInt16),
			"UInt16[]" => ReadArray(length, binaryReader.ReadUInt16),
			"Int32[]" => ReadArray(length, binaryReader.ReadInt32),
			"UInt32[]" => ReadArray(length, binaryReader.ReadUInt32),
			"Int64[]" => ReadArray(length, binaryReader.ReadInt64),
			"UInt64[]" => ReadArray(length, binaryReader.ReadUInt64),
			"Single[]" => ReadArray(length, binaryReader.ReadSingle),
			"Double[]" => ReadArray(length, binaryReader.ReadDouble),
			"Decimal[]" => ReadArray(length, binaryReader.ReadDecimal),
			"String[]" => ReadArray(length, binaryReader.ReadString),
			"DateTime[]" => ReadArray(length, () => DateTime.FromBinary(binaryReader.ReadInt64())),
			"TimeSpan[]" => ReadArray(length, () => TimeSpan.FromTicks(binaryReader.ReadInt64())),
			_ when typeof(T).IsArray => ReadArray(length, typeof(T).GetElementType()!),
			_ => ReadObject(typeof(T)),
		};

		return (T?)@object;
	}

	private object ReadObject(Type type)
	{
		if (binaryReader.ReadChar() != '{') throw new Exception();
		IBox box = (IBox)RuntimeHelpers.GetUninitializedObject(type);
		box.ReadFrom(this);
		if (binaryReader.ReadChar() != '}') throw new Exception();
		return box;
	}

	private T[] ReadArray<T>(int length, Func<T> func)
	{
		T[] array = new T[length];
		for (int i = 0; i < length; i++) array[i] = func();
		return array;
	}

	private Array ReadArray(int length, Type type)
	{
		Array array = Array.CreateInstance(type, length);
		for (int i = 0; i < length; i++) array.SetValue(ReadObject(type), i);
		return array;
	}
}