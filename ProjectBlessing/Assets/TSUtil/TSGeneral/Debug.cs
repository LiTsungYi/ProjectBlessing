#if DEF_DISABLE_DEBUGLOG && !UNITY_EDITOR
using System.Diagnostics;
using UnityEngine;

public static class Debug
{
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void Break(){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void ClearDeveloperConsole(){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DebugBreak(){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawLine(Vector3 start, Vector3 end){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawLine(Vector3 start, Vector3 end, Color color){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawRay(Vector3 start, Vector3 dir){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawRay(Vector3 start, Vector3 dir, Color color){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest){}
	
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void Log(object message){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void Log(object message, UnityEngine.Object context){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void LogError(object message){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void LogError(object message, UnityEngine.Object context){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void LogException(System.Exception exception){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void LogException(System.Exception exception, UnityEngine.Object context){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void LogWarning(object message){}
	[Conditional("DEF_DISABLE_DEBUGLOG")] public static void LogWarning(object message, UnityEngine.Object context){}
}
#endif