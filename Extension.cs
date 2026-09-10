using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BOX;

public static class Extension
{
	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
	{
		foreach (T t in enumerable)
			action(t);
	}

	public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T, int> action)
	{
		int index = 0;

		foreach (T t in enumerable)
			action(t, index++);
	}

	public static async Task ForEach<T>(this IEnumerable<T> enumerable, Func<T, Task> action)
	{
		foreach (T t in enumerable)
			await action(t);
	}

	public static async Task<T[]> WhenAll<T>(this IEnumerable<Task<T>> tasks)
	{
		T[] array = await Task.WhenAll(tasks);
		return array;
	}

	public static bool IsValidEmail(this string? email)
	{
		if (string.IsNullOrWhiteSpace(email)) return false;
		if (email.Contains(' ')) return false;
		int atIndex = email.IndexOf('@');
		int dotIndex = email.LastIndexOf('.');
		return atIndex > 0 && dotIndex > atIndex + 1 && dotIndex < email.Length - 1;
	}
}