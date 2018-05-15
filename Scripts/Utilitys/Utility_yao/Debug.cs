using System.Diagnostics;
using UnityEngine;

// 在 Symbol 那邊把 DEBUG_FILTER 填入
// 會把所有的 Debug 全部用空方法取代
// 意即關閉所有 Debug log

#if DEBUG_FILTER
public static class Debug
{
    [Conditional("DEBUG_FILTER")]
    public static void Break() { }
    [Conditional("DEBUG_FILTER")]
    public static void ClearDeveloperConsole() { }
    [Conditional("DEBUG_FILTER")]
    public static void DebugBreak() { }

    [Conditional("DEBUG_FILTER")]
    public static void DrawLine(Vector3 start, Vector3 end) { }
    [Conditional("DEBUG_FILTER")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color) { }
    [Conditional("DEBUG_FILTER")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration) { }
    [Conditional("DEBUG_FILTER")]
    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration, bool depthTest) { }

    [Conditional("DEBUG_FILTER")]
    public static void DrawRay(Vector3 start, Vector3 dir) { }
    [Conditional("DEBUG_FILTER")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color) { }
    [Conditional("DEBUG_FILTER")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration) { }
    [Conditional("DEBUG_FILTER")]
    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration, bool depthTest) { }

    [Conditional("DEBUG_FILTER")]
    public static void Log(object message) { }
    [Conditional("DEBUG_FILTER")]
    public static void Log(object message, Object context) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogFormat(string format, params object[] args) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogFormat(Object context, string format, params object[] args) { }

    [Conditional("DEBUG_FILTER")]
    public static void LogWarning(object message) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogWarning(object message, Object context) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogWarningFormat(string format, params object[] args) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogWarningFormat(Object context, string format, params object[] args) { }

    [Conditional("DEBUG_FILTER")]
    public static void LogError(object message) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogError(object message, Object context) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogErrorFormat(string format, params object[] args) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogErrorFormat(Object context, string format, params object[] args) { }

    [Conditional("DEBUG_FILTER")]
    public static void LogException(System.Exception exception) { }
    [Conditional("DEBUG_FILTER")]
    public static void LogException(System.Exception exception, Object context) { }
}
#endif