namespace BOX;

public class Box<T> : IBox
{
	private T value;

	public T Value => value;

	public Box(T value)
	{
		this.value = value;
	}

	public void WriteTo(IWriter writer)
	{
		writer.Write(value);
	}

	public void ReadFrom(IReader reader)
	{
		reader.Read(out value);
	}
}