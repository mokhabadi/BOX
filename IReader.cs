using System.Runtime.CompilerServices;

namespace BOX;

public interface IReader
{
	public void Read<T>(out T value, [CallerArgumentExpression(nameof(value))] string key = "");
	public T Read<T>(T? _, [CallerArgumentExpression(nameof(_))] string key = "");
}