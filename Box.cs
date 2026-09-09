namespace BOX;

public class Box<T> : IBox
{
	public T Value { get; private set; }

	public Box(T value)
	{
		Value = value;
	}

	public void WriteTo(IWriter writer)
	{
		writer.Write(Value);
	}

	public void ReadFrom(IReader reader)
	{
		Value = reader.Read(Value);
	}
}