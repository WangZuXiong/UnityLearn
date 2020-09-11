using System.Diagnostics;
using UnityEngine;

#if !UNITY_EDITOR
public static class Debug
{
    /* UnityEngine.Debug过滤级别为Log的情况下，即UnityEngine.Debug.unityLogger.filterLogType = LogType.Log;
     * 添加 DEBUG_ENABLE 宏，Log、LogWarning、LogError都能输出
     * 移除 DEBUG_ENABLE 宏，LogError能输出，Log、LogWarning相关方法以及调用将会被移除，不被编译到包内
     */

    // Log
    [Conditional("DEBUG_ENABLE")]
    public static void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
    [Conditional("DEBUG_ENABLE")]
    public static void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }
    [Conditional("DEBUG_ENABLE")]
    public static void LogFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(format, args);
    }
    [Conditional("DEBUG_ENABLE")]
    public static void LogFormat(Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, format, args);
    }

    // Warning
    [Conditional("DEBUG_ENABLE")]
    public static void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }

    [Conditional("DEBUG_ENABLE")]
    public static void LogWarning(object message, Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }

    [Conditional("DEBUG_ENABLE")]
    public static void LogWarningFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(format, args);
    }

    [Conditional("DEBUG_ENABLE")]
    public static void LogWarningFormat(Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

    // Error

    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }

    public static void LogError(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    public static void LogErrorFormat(string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(format, args);
    }

    public static void LogErrorFormat(Object context, string format, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(context, format, args);     
    }
}
#endif