using System.Runtime.CompilerServices;

namespace BOX;

public interface IWriter
{
	public void Write<T>(T? value, [CallerArgumentExpression(nameof(value))] string key = "");
}