using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Box;

public class Tester
{
	private readonly bool boolValue = true;
	private readonly char charValue = 'M';
	private readonly byte byteValue = 199;
	private readonly sbyte sbyteValue = -77;
	private readonly short shortValue = 32000;
	private readonly ushort ushortValue = 64000;
	private readonly int intValue = -123456;
	private readonly uint uintValue = 654321;
	private readonly long longValue = -9876543210;
	private readonly ulong ulongValue = 9123456789;
	private readonly float floatValue = 55.55f;
	private readonly double doubleValue = 22.22;
	private readonly decimal decimalValue = 66.66m;
	private readonly string stringValue = "Hello World";
	private readonly DateTime dateTimeValue = new(2000, 1, 1);
	private readonly TimeSpan timeSpanValue = new(10, 10, 10);
	private readonly TestBox testBoxValue = new(333, "test_box", new([4.4f, 4.5f, 4.6f]));

	private readonly bool? boolNull = null;
	private readonly char? charNull = null;
	private readonly byte? byteNull = null;
	private readonly sbyte? sbyteNull = null;
	private readonly short? shortNull = null;
	private readonly ushort? ushortNull = null;
	private readonly int? intNull = null;
	private readonly uint? uintNull = null;
	private readonly long? longNull = null;
	private readonly ulong? ulongNull = null;
	private readonly float? floatNull = null;
	private readonly double? doubleNull = null;
	private readonly decimal? decimalNull = null;
	private readonly string? stringNull = null;
	private readonly DateTime? dateTimeNull = null;
	private readonly TimeSpan? timeSpanNull = null;
	private readonly TestBox? testBoxNull = null;

	private readonly bool? boolNonNull = true;
	private readonly char? charNonNull = 'M';
	private readonly byte? byteNonNull = 199;
	private readonly sbyte? sbyteNonNull = -77;
	private readonly short? shortNonNull = 32000;
	private readonly ushort? ushortNonNull = 64000;
	private readonly int? intNonNull = -123456;
	private readonly uint? uintNonNull = 654321;
	private readonly long? longNonNull = -9876543210;
	private readonly ulong? ulongNonNull = 9123456789;
	private readonly float? floatNonNull = 55.55f;
	private readonly double? doubleNonNull = 22.22;
	private readonly decimal? decimalNonNull = 66.66m;
	private readonly string? stringNonNull = "Hello World";
	private readonly DateTime? dateTimeNonNull = new(2000, 1, 1);
	private readonly TimeSpan? timeSpanNonNull = new(10, 10, 10);
	private readonly TestBox? testBoxNonNull = new(333, "test_box", new([4.4f, 4.5f, 4.6f]));

	private readonly bool[] boolArray = [true, false, true, false];
	private readonly char[] charArray = ['A', 'B', 'C', 'D', 'E', 'F'];
	private readonly byte[] byteArray = [10, 20, 30, 40];
	private readonly sbyte[] sbyteArray = [-10, -20, -30, -40];
	private readonly short[] shortArray = [1000, 10000];
	private readonly ushort[] ushortArray = [6000, 60000];
	private readonly int[] intArray = [999999, -99];
	private readonly uint[] uintArray = [uint.MaxValue, 43];
	private readonly long[] longArray = [10123000123, -23];
	private readonly ulong[] ulongArray = [ulong.MaxValue, 31];
	private readonly float[] floatArray = [1.1f, 2.2f];
	private readonly double[] doubleArray = [0.123, 0.456];
	private readonly decimal[] decimalArray = [9.99m, 7.99m];
	private readonly string[] stringArray = ["alice", "bob"];
	private readonly DateTime[] dateTimeArray = [new(2026, 6, 6), new(2027, 7, 7)];
	private readonly TimeSpan[] timeSpanArray = [TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)];
	private readonly TestBox[] testBoxArray = [new(1, "one", new([1, 1, 1])), new(2, "two", new([2, 2, 2]))];

	private readonly bool[]? boolNullArray = null;
	private readonly char[]? charNullArray = null;
	private readonly byte[]? byteNullArray = null;
	private readonly sbyte[]? sbyteNullArray = null;
	private readonly short[]? shortNullArray = null;
	private readonly ushort[]? ushortNullArray = null;
	private readonly int[]? intNullArray = null;
	private readonly uint[]? uintNullArray = null;
	private readonly long[]? longNullArray = null;
	private readonly ulong[]? ulongNullArray = null;
	private readonly float[]? floatNullArray = null;
	private readonly double[]? doubleNullArray = null;
	private readonly decimal[]? decimalNullArray = null;
	private readonly string[]? stringNullArray = null;
	private readonly DateTime[]? dateTimeNullArray = null;
	private readonly TimeSpan[]? timeSpanNullArray = null;
	private readonly TestBox[]? testBoxNullArray = null;

	private readonly bool[]? boolNonNullArray = [true, false, true, false];
	private readonly char[]? charNonNullArray = ['A', 'B', 'C', 'D', 'E', 'F'];
	private readonly byte[]? byteNonNullArray = [10, 20, 30, 40];
	private readonly sbyte[]? sbyteNonNullArray = [-10, -20, -30, -40];
	private readonly short[]? shortNonNullArray = [1000, 10000];
	private readonly ushort[]? ushortNonNullArray = [6000, 60000];
	private readonly int[]? intNonNullArray = [999999, -99];
	private readonly uint[]? uintNonNullArray = [uint.MaxValue, 43];
	private readonly long[]? longNonNullArray = [10123000123, -23];
	private readonly ulong[]? ulongNonNullArray = [ulong.MaxValue, 31];
	private readonly float[]? floatNonNullArray = [1.1f, 2.2f];
	private readonly double[]? doubleNonNullArray = [0.123, 0.456];
	private readonly decimal[]? decimalNonNullArray = [9.99m, 7.99m];
	private readonly string[]? stringNonNullArray = ["alice", "bob"];
	private readonly DateTime[]? dateTimeNonNullArray = [new(2026, 6, 6), new(2027, 7, 7)];
	private readonly TimeSpan[]? timeSpanNonNullArray = [TimeSpan.FromSeconds(5), TimeSpan.FromMinutes(30)];
	private readonly TestBox[]? testBoxNonNullArray = [new(1, "one", new([1, 1.1f, 1.2f])), new(2, "two", new([2, 2.1f, 2.2f]))];
	private readonly int[][] array2D = [[1, 2], [3, 4]];

	public void Execute()
	{
		MemoryStream memoryStream = new();
		Write(memoryStream);
		memoryStream.Position = 0;
		byte[] bytes = memoryStream.ToArray();
		string box = string.Concat(bytes.Select(b => b is >= 32 and <= 126 ? $"{(char)b}" : $"{b}"));
		Trace.WriteLine(box);
		Printer printer = new(new BinaryReader(memoryStream));
		string print = printer.Print();
		Trace.WriteLine(print);
		memoryStream.Position = 0;
		Read(memoryStream);
	}

	private void Write(MemoryStream stream)
	{
		BinaryWriter binaryWriter = new(stream);
		IWriter writer = new Writer(binaryWriter);

		writer.Write(boolValue);
		writer.Write(charValue);
		writer.Write(byteValue);
		writer.Write(sbyteValue);
		writer.Write(shortValue);
		writer.Write(ushortValue);
		writer.Write(intValue);
		writer.Write(uintValue);
		writer.Write(longValue);
		writer.Write(ulongValue);
		writer.Write(floatValue);
		writer.Write(doubleValue);
		writer.Write(decimalValue);
		writer.Write(stringValue);
		writer.Write(dateTimeValue);
		writer.Write(timeSpanValue);
		writer.Write(testBoxValue);

		writer.Write(boolNull);
		writer.Write(charNull);
		writer.Write(byteNull);
		writer.Write(sbyteNull);
		writer.Write(shortNull);
		writer.Write(ushortNull);
		writer.Write(intNull);
		writer.Write(uintNull);
		writer.Write(longNull);
		writer.Write(ulongNull);
		writer.Write(floatNull);
		writer.Write(doubleNull);
		writer.Write(decimalNull);
		writer.Write(stringNull);
		writer.Write(dateTimeNull);
		writer.Write(timeSpanNull);
		writer.Write(testBoxNull);

		writer.Write(boolNonNull);
		writer.Write(charNonNull);
		writer.Write(byteNonNull);
		writer.Write(sbyteNonNull);
		writer.Write(shortNonNull);
		writer.Write(ushortNonNull);
		writer.Write(intNonNull);
		writer.Write(uintNonNull);
		writer.Write(longNonNull);
		writer.Write(ulongNonNull);
		writer.Write(floatNonNull);
		writer.Write(doubleNonNull);
		writer.Write(decimalNonNull);
		writer.Write(stringNonNull);
		writer.Write(dateTimeNonNull);
		writer.Write(timeSpanNonNull);
		writer.Write(testBoxNonNull);

		writer.Write(boolArray);
		writer.Write(charArray);
		writer.Write(byteArray);
		writer.Write(sbyteArray);
		writer.Write(shortArray);
		writer.Write(ushortArray);
		writer.Write(intArray);
		writer.Write(uintArray);
		writer.Write(longArray);
		writer.Write(ulongArray);
		writer.Write(floatArray);
		writer.Write(doubleArray);
		writer.Write(decimalArray);
		writer.Write(stringArray);
		writer.Write(dateTimeArray);
		writer.Write(timeSpanArray);
		writer.Write(testBoxArray);

		writer.Write(boolNullArray);
		writer.Write(charNullArray);
		writer.Write(byteNullArray);
		writer.Write(sbyteNullArray);
		writer.Write(shortNullArray);
		writer.Write(ushortNullArray);
		writer.Write(intNullArray);
		writer.Write(uintNullArray);
		writer.Write(longNullArray);
		writer.Write(ulongNullArray);
		writer.Write(floatNullArray);
		writer.Write(doubleNullArray);
		writer.Write(decimalNullArray);
		writer.Write(stringNullArray);
		writer.Write(dateTimeNullArray);
		writer.Write(timeSpanNullArray);
		writer.Write(testBoxNullArray);

		writer.Write(boolNonNullArray);
		writer.Write(charNonNullArray);
		writer.Write(byteNonNullArray);
		writer.Write(sbyteNonNullArray);
		writer.Write(shortNonNullArray);
		writer.Write(ushortNonNullArray);
		writer.Write(intNonNullArray);
		writer.Write(uintNonNullArray);
		writer.Write(longNonNullArray);
		writer.Write(ulongNonNullArray);
		writer.Write(floatNonNullArray);
		writer.Write(doubleNonNullArray);
		writer.Write(decimalNonNullArray);
		writer.Write(stringNonNullArray);
		writer.Write(dateTimeNonNullArray);
		writer.Write(timeSpanNonNullArray);
		writer.Write(testBoxNonNullArray);

		writer.Write(testBoxValue);
		writer.Write(testBoxNull);
		writer.Write(testBoxNonNull);
		writer.Write(testBoxArray);
		writer.Write(testBoxNullArray);
		writer.Write(testBoxNonNullArray);
		writer.Write(array2D);
	}

	private void Read(MemoryStream stream)
	{
		BinaryReader binaryReader = new(stream);
		IReader reader = new Reader(binaryReader);

		reader.Read(out bool boolValue);
		reader.Read(out char charValue);
		reader.Read(out byte byteValue);
		reader.Read(out sbyte sbyteValue);
		reader.Read(out short shortValue);
		reader.Read(out ushort ushortValue);
		reader.Read(out int intValue);
		reader.Read(out uint uintValue);
		reader.Read(out long longValue);
		reader.Read(out ulong ulongValue);
		reader.Read(out float floatValue);
		reader.Read(out double doubleValue);
		reader.Read(out decimal decimalValue);
		reader.Read(out string stringValue);
		reader.Read(out DateTime dateTimeValue);
		reader.Read(out TimeSpan timeSpanValue);
		reader.Read(out TestBox testBoxValue);

		reader.Read(out bool? boolNull);
		reader.Read(out char? charNull);
		reader.Read(out byte? byteNull);
		reader.Read(out sbyte? sbyteNull);
		reader.Read(out short? shortNull);
		reader.Read(out ushort? ushortNull);
		reader.Read(out int? intNull);
		reader.Read(out uint? uintNull);
		reader.Read(out long? longNull);
		reader.Read(out ulong? ulongNull);
		reader.Read(out float? floatNull);
		reader.Read(out double? doubleNull);
		reader.Read(out decimal? decimalNull);
		reader.Read(out string? stringNull);
		reader.Read(out DateTime? dateTimeNull);
		reader.Read(out TimeSpan? timeSpanNull);
		reader.Read(out TestBox? testBoxNull);

		reader.Read(out bool? boolNonNull);
		reader.Read(out char? charNonNull);
		reader.Read(out byte? byteNonNull);
		reader.Read(out sbyte? sbyteNonNull);
		reader.Read(out short? shortNonNull);
		reader.Read(out ushort? ushortNonNull);
		reader.Read(out int? intNonNull);
		reader.Read(out uint? uintNonNull);
		reader.Read(out long? longNonNull);
		reader.Read(out ulong? ulongNonNull);
		reader.Read(out float? floatNonNull);
		reader.Read(out double? doubleNonNull);
		reader.Read(out decimal? decimalNonNull);
		reader.Read(out string? stringNonNull);
		reader.Read(out DateTime? dateTimeNonNull);
		reader.Read(out TimeSpan? timeSpanNonNull);
		reader.Read(out TestBox? testBoxNonNull);

		reader.Read(out bool[] boolArray);
		reader.Read(out char[] charArray);
		reader.Read(out byte[] byteArray);
		reader.Read(out sbyte[] sbyteArray);
		reader.Read(out short[] shortArray);
		reader.Read(out ushort[] ushortArray);
		reader.Read(out int[] intArray);
		reader.Read(out uint[] uintArray);
		reader.Read(out long[] longArray);
		reader.Read(out ulong[] ulongArray);
		reader.Read(out float[] floatArray);
		reader.Read(out double[] doubleArray);
		reader.Read(out decimal[] decimalArray);
		reader.Read(out string[] stringArray);
		reader.Read(out DateTime[] dateTimeArray);
		reader.Read(out TimeSpan[] timeSpanArray);
		reader.Read(out TestBox[] testBoxArray);

		reader.Read(out bool[]? boolNullArray);
		reader.Read(out char[]? charNullArray);
		reader.Read(out byte[]? byteNullArray);
		reader.Read(out sbyte[]? sbyteNullArray);
		reader.Read(out short[]? shortNullArray);
		reader.Read(out ushort[]? ushortNullArray);
		reader.Read(out int[]? intNullArray);
		reader.Read(out uint[]? uintNullArray);
		reader.Read(out long[]? longNullArray);
		reader.Read(out ulong[]? ulongNullArray);
		reader.Read(out float[]? floatNullArray);
		reader.Read(out double[]? doubleNullArray);
		reader.Read(out decimal[]? decimalNullArray);
		reader.Read(out string[]? stringNullArray);
		reader.Read(out DateTime[]? dateTimeNullArray);
		reader.Read(out TimeSpan[]? timeSpanNullArray);
		reader.Read(out TestBox[]? testBoxNullArray);

		reader.Read(out bool[]? boolNonNullArray);
		reader.Read(out char[]? charNonNullArray);
		reader.Read(out byte[]? byteNonNullArray);
		reader.Read(out sbyte[]? sbyteNonNullArray);
		reader.Read(out short[]? shortNonNullArray);
		reader.Read(out ushort[]? ushortNonNullArray);
		reader.Read(out int[]? intNonNullArray);
		reader.Read(out uint[]? uintNonNullArray);
		reader.Read(out long[]? longNonNullArray);
		reader.Read(out ulong[]? ulongNonNullArray);
		reader.Read(out float[]? floatNonNullArray);
		reader.Read(out double[]? doubleNonNullArray);
		reader.Read(out decimal[]? decimalNonNullArray);
		reader.Read(out string[]? stringNonNullArray);
		reader.Read(out DateTime[]? dateTimeNonNullArray);
		reader.Read(out TimeSpan[]? timeSpanNonNullArray);
		reader.Read(out TestBox[]? testBoxNonNullArray);

		TestBox testBox1 = reader.Read<TestBox>(null, nameof(testBoxValue));
		TestBox? testBox2 = reader.Read<TestBox?>(null, nameof(testBoxNull));
		TestBox? testBox3 = reader.Read<TestBox?>(null, nameof(testBoxNonNull));
		TestBox[] testBoxes1 = reader.Read<TestBox[]>(null, nameof(testBoxArray));
		TestBox[]? testBoxes2 = reader.Read<TestBox[]?>(null, nameof(testBoxNullArray));
		TestBox[]? testBoxes3 = reader.Read<TestBox[]?>(null, nameof(testBoxNonNullArray));
		reader.Read(out int[][] array2D);

		AssertEquality(this.boolValue, boolValue);
		AssertEquality(this.charValue, charValue);
		AssertEquality(this.byteValue, byteValue);
		AssertEquality(this.sbyteValue, sbyteValue);
		AssertEquality(this.shortValue, shortValue);
		AssertEquality(this.ushortValue, ushortValue);
		AssertEquality(this.intValue, intValue);
		AssertEquality(this.uintValue, uintValue);
		AssertEquality(this.longValue, longValue);
		AssertEquality(this.ulongValue, ulongValue);
		AssertEquality(this.floatValue, floatValue);
		AssertEquality(this.doubleValue, doubleValue);
		AssertEquality(this.decimalValue, decimalValue);
		AssertEquality(this.stringValue, stringValue);
		AssertEquality(this.dateTimeValue, dateTimeValue);
		AssertEquality(this.timeSpanValue, timeSpanValue);
		AssertEquality(this.testBoxValue, testBoxValue);

		AssertEquality(this.boolNull, boolNull);
		AssertEquality(this.charNull, charNull);
		AssertEquality(this.byteNull, byteNull);
		AssertEquality(this.sbyteNull, sbyteNull);
		AssertEquality(this.shortNull, shortNull);
		AssertEquality(this.ushortNull, ushortNull);
		AssertEquality(this.intNull, intNull);
		AssertEquality(this.uintNull, uintNull);
		AssertEquality(this.longNull, longNull);
		AssertEquality(this.ulongNull, ulongNull);
		AssertEquality(this.floatNull, floatNull);
		AssertEquality(this.doubleNull, doubleNull);
		AssertEquality(this.decimalNull, decimalNull);
		AssertEquality(this.stringNull, stringNull);
		AssertEquality(this.dateTimeNull, dateTimeNull);
		AssertEquality(this.timeSpanNull, timeSpanNull);
		AssertEquality(this.testBoxNull, testBoxNull);

		AssertEquality(this.boolNonNull, boolNonNull);
		AssertEquality(this.charNonNull, charNonNull);
		AssertEquality(this.byteNonNull, byteNonNull);
		AssertEquality(this.sbyteNonNull, sbyteNonNull);
		AssertEquality(this.shortNonNull, shortNonNull);
		AssertEquality(this.ushortNonNull, ushortNonNull);
		AssertEquality(this.intNonNull, intNonNull);
		AssertEquality(this.uintNonNull, uintNonNull);
		AssertEquality(this.longNonNull, longNonNull);
		AssertEquality(this.ulongNonNull, ulongNonNull);
		AssertEquality(this.floatNonNull, floatNonNull);
		AssertEquality(this.doubleNonNull, doubleNonNull);
		AssertEquality(this.decimalNonNull, decimalNonNull);
		AssertEquality(this.stringNonNull, stringNonNull);
		AssertEquality(this.dateTimeNonNull, dateTimeNonNull);
		AssertEquality(this.timeSpanNonNull, timeSpanNonNull);
		AssertEquality(this.testBoxNonNull, testBoxNonNull);

		AssertEquality(this.boolArray, boolArray);
		AssertEquality(this.charArray, charArray);
		AssertEquality(this.byteArray, byteArray);
		AssertEquality(this.sbyteArray, sbyteArray);
		AssertEquality(this.shortArray, shortArray);
		AssertEquality(this.ushortArray, ushortArray);
		AssertEquality(this.intArray, intArray);
		AssertEquality(this.uintArray, uintArray);
		AssertEquality(this.longArray, longArray);
		AssertEquality(this.ulongArray, ulongArray);
		AssertEquality(this.floatArray, floatArray);
		AssertEquality(this.doubleArray, doubleArray);
		AssertEquality(this.decimalArray, decimalArray);
		AssertEquality(this.stringArray, stringArray);
		AssertEquality(this.dateTimeArray, dateTimeArray);
		AssertEquality(this.timeSpanArray, timeSpanArray);
		AssertEquality(this.testBoxArray, testBoxArray);

		AssertEquality(this.boolNullArray, boolNullArray);
		AssertEquality(this.charNullArray, charNullArray);
		AssertEquality(this.byteNullArray, byteNullArray);
		AssertEquality(this.sbyteNullArray, sbyteNullArray);
		AssertEquality(this.shortNullArray, shortNullArray);
		AssertEquality(this.ushortNullArray, ushortNullArray);
		AssertEquality(this.intNullArray, intNullArray);
		AssertEquality(this.uintNullArray, uintNullArray);
		AssertEquality(this.longNullArray, longNullArray);
		AssertEquality(this.ulongNullArray, ulongNullArray);
		AssertEquality(this.floatNullArray, floatNullArray);
		AssertEquality(this.doubleNullArray, doubleNullArray);
		AssertEquality(this.decimalNullArray, decimalNullArray);
		AssertEquality(this.stringNullArray, stringNullArray);
		AssertEquality(this.dateTimeNullArray, dateTimeNullArray);
		AssertEquality(this.timeSpanNullArray, timeSpanNullArray);
		AssertEquality(this.testBoxNullArray, testBoxNullArray);

		AssertEquality(this.boolNonNullArray, boolNonNullArray);
		AssertEquality(this.charNonNullArray, charNonNullArray);
		AssertEquality(this.byteNonNullArray, byteNonNullArray);
		AssertEquality(this.sbyteNonNullArray, sbyteNonNullArray);
		AssertEquality(this.shortNonNullArray, shortNonNullArray);
		AssertEquality(this.ushortNonNullArray, ushortNonNullArray);
		AssertEquality(this.intNonNullArray, intNonNullArray);
		AssertEquality(this.uintNonNullArray, uintNonNullArray);
		AssertEquality(this.longNonNullArray, longNonNullArray);
		AssertEquality(this.ulongNonNullArray, ulongNonNullArray);
		AssertEquality(this.floatNonNullArray, floatNonNullArray);
		AssertEquality(this.doubleNonNullArray, doubleNonNullArray);
		AssertEquality(this.decimalNonNullArray, decimalNonNullArray);
		AssertEquality(this.stringNonNullArray, stringNonNullArray);
		AssertEquality(this.dateTimeNonNullArray, dateTimeNonNullArray);
		AssertEquality(this.timeSpanNonNullArray, timeSpanNonNullArray);
		AssertEquality(this.testBoxNonNullArray, testBoxNonNullArray);

		AssertEquality(testBox1, testBoxValue);
		AssertEquality(testBox2, testBoxNull);
		AssertEquality(testBox3, testBoxNonNull);
		AssertEquality(testBoxes1, testBoxArray);
		AssertEquality(testBoxes2, testBoxNullArray);
		AssertEquality(testBoxes3, testBoxNonNullArray);

		for (int i = 0; i < array2D.Length; i++) AssertEquality(this.array2D[i], array2D[i]);
	}

	private static void AssertEquality<T>(T t1, T t2)
	{
		if (!Equals(t1, t2)) throw new Exception($"value mismatch '{t1}' and '{t2}'");
	}

	private static void AssertEquality<T>(T[]? t1, T[]? t2)
	{
		if (t1 == t2) return;
		if (t1 == null || t2 == null || !t1.SequenceEqual(t2)) throw new Exception($"value mismatch '{t1}' and '{t2}'");
	}

	private class TestBox : IBox
	{
		private int Id { get; set; }
		private string Name { get; set; }
		private InnerBox InnerBox { get; set; }

		public TestBox(int id, string name, InnerBox innerBox)
		{
			Id = id;
			Name = name;
			InnerBox = innerBox;
		}

		public void WriteTo(IWriter writer)
		{
			writer.Write(Id);
			writer.Write(Name);
			writer.Write(InnerBox);
		}

		public void ReadFrom(IReader reader)
		{
			Id = reader.Read(Id);
			Name = reader.Read(Name);
			InnerBox = reader.Read(InnerBox);
		}

		public override bool Equals(object? obj)
		{
			return obj is TestBox testBox && Id == testBox.Id && Name == testBox.Name && InnerBox.Equals(testBox.InnerBox);
		}

		public override int GetHashCode() => 0;

		public override string ToString() => $"id: {Id}, name: {Name}, innerBox: {InnerBox}";
	}

	private class InnerBox : IBox
	{
		private float[] Dimensions { get; set; }

		public InnerBox(float[] dimensions)
		{
			Dimensions = dimensions;
		}

		public void WriteTo(IWriter writer)
		{
			writer.Write(Dimensions);
		}

		public void ReadFrom(IReader reader)
		{
			Dimensions = reader.Read(Dimensions);
		}

		public override bool Equals(object? obj)
		{
			return obj is InnerBox other && Dimensions.SequenceEqual(other.Dimensions);
		}

		public override int GetHashCode() => 0;

		public override string ToString() => $"[{string.Join(',', Dimensions)}]')]";
	}
}