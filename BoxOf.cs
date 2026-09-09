namespace BOX;

public class BoxOf<T> : IBox
{
	private T value;

	public T Value => value;

	public BoxOf(T value)
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