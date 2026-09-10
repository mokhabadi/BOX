using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace BOX;

public static class Output
{
	private static Action<string> action = null!;

	public static void SetAction(Action<string> action)
	{
		Output.action = action;
	}

	public static void Write(object message, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = 0)
	{
		string dateTimeString = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss.fff}]";
		string callerType = Path.GetFileNameWithoutExtension(callerFilePath);
		string callerInfo = $"[{callerType}.{callerMemberName}: line {callerLineNumber}]";
		string logMessage = $"{dateTimeString} {callerInfo}: {message as Exception ?? message}";
		action(logMessage);
	}
}