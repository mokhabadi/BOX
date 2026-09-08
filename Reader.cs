using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace Box;

public class Reader(BinaryReader binaryReader) : IReader
{
	public void Read<T>(out T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		string expectedKey = binaryReader.ReadString();
		key = key[(key.LastIndexOf(' ') + 1)..];
		if (key != expectedKey) throw new Exception($"value mismatch '{key}', expected '{expectedKey}'");
		string typeName = binaryReader.ReadString();
		Type type = Nullable.GetUnderlyingType(typeof(T)) ?? typeof(T);
		if (typeName != type.Name) throw new Exception($"type mismatch '{type.Name}', expected '{typeName}'");
		char valueSign = binaryReader.ReadChar();
		if (valueSign is not ('=' or '~')) throw new Exception();
		value = valueSign == '=' ? ReadValue<T>(type) : default;
		if (binaryReader.ReadChar() != ';') throw new Exception();
	}

	public T? Read<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "")
	{
		Read(out T? t, key);
		return t;
	}

	private T ReadValue<T>(Type type)
	{
		if (type == typeof(bool)) return (T)(object)binaryReader.ReadBoolean();
		if (type == typeof(char)) return (T)(object)binaryReader.ReadChar();
		if (type == typeof(byte)) return (T)(object)binaryReader.ReadByte();
		if (type == typeof(sbyte)) return (T)(object)binaryReader.ReadSByte();
		if (type == typeof(short)) return (T)(object)binaryReader.ReadInt16();
		if (type == typeof(ushort)) return (T)(object)binaryReader.ReadUInt16();
		if (type == typeof(int)) return (T)(object)binaryReader.ReadInt32();
		if (type == typeof(uint)) return (T)(object)binaryReader.ReadUInt32();
		if (type == typeof(long)) return (T)(object)binaryReader.ReadInt64();
		if (type == typeof(ulong)) return (T)(object)binaryReader.ReadUInt64();
		if (type == typeof(float)) return (T)(object)binaryReader.ReadSingle();
		if (type == typeof(double)) return (T)(object)binaryReader.ReadDouble();
		if (type == typeof(decimal)) return (T)(object)binaryReader.ReadDecimal();
		if (type == typeof(string)) return (T)(object)binaryReader.ReadString();
		if (type == typeof(DateTime)) return (T)(object)DateTime.FromBinary(binaryReader.ReadInt64());
		if (type == typeof(TimeSpan)) return (T)(object)TimeSpan.FromTicks(binaryReader.ReadInt64());
		if (type.IsAssignableTo(typeof(IBox))) return (T)ReadObject(type);
		if (!type.IsArray) throw new NotSupportedException(type.FullName);
		int length = binaryReader.Read7BitEncodedInt();
		if (type == typeof(bool[])) return (T)(object)ReadArray(length, binaryReader.ReadBoolean);
		if (type == typeof(char[])) return (T)(object)binaryReader.ReadChars(length);
		if (type == typeof(byte[]) || type == typeof(sbyte[])) return (T)(object)binaryReader.ReadBytes(length);
		if (type == typeof(short[]) || type == typeof(ushort[])) return (T)(object)ReadArray(length, binaryReader.ReadInt16);
		if (type == typeof(int[]) || type == typeof(uint[])) return (T)(object)ReadArray(length, binaryReader.ReadInt32);
		if (type == typeof(long[]) || type == typeof(ulong[])) return (T)(object)ReadArray(length, binaryReader.ReadInt64);
		if (type == typeof(float[])) return (T)(object)ReadArray(length, binaryReader.ReadSingle);
		if (type == typeof(double[])) return (T)(object)ReadArray(length, binaryReader.ReadDouble);
		if (type == typeof(decimal[])) return (T)(object)ReadArray(length, binaryReader.ReadDecimal);
		if (type == typeof(string[])) return (T)(object)ReadArray(length, binaryReader.ReadString);
		if (type == typeof(DateTime[])) return (T)(object)ReadArray(length, () => DateTime.FromBinary(binaryReader.ReadInt64()));
		if (type == typeof(TimeSpan[])) return (T)(object)ReadArray(length, () => TimeSpan.FromTicks(binaryReader.ReadInt64()));
		if (type.IsAssignableTo(typeof(IBox[]))) return (T)(object)ReadArray(length, type.GetElementType()!);
		throw new NotSupportedException(type.FullName);
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
		if (binaryReader.ReadChar() != '[') throw new Exception();
		T[] array = new T[length];

		for (int i = 0; i < length; i++)
		{
			if (binaryReader.ReadChar() != ',') throw new Exception();
			array[i] = func();
		}

		if (binaryReader.ReadChar() != ']') throw new Exception();
		return array;
	}

	private Array ReadArray(int length, Type type)
	{
		if (binaryReader.ReadChar() != '[') throw new Exception();
		Array array = Array.CreateInstance(type, length);

		for (int i = 0; i < length; i++)
		{
			if (binaryReader.ReadChar() != ',') throw new Exception();
			array.SetValue(ReadObject(type), i);
		}

		if (binaryReader.ReadChar() != ']') throw new Exception();
		return array;
	}
}