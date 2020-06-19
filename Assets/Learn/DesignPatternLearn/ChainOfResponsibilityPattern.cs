using UnityEngine;
/// <summary>
/// 责任链模式
/// </summary>
public class ChainOfResponsibilityPattern : MonoBehaviour
{
    public abstract class AbstractLogger
    {
        public static int Debug = 1;
        public static int Warning = 2;
        public static int Error = 3;

        protected int Level;

        protected AbstractLogger NextLogger;

        public void SetNextLogger(AbstractLogger nextLogger)
        {
            NextLogger = nextLogger;
        }

        public void LogMessage(int level, string message)
        {
            if (Level <= level)
            {
                Write(message);
            }

            if (NextLogger != null)
            {
                NextLogger.LogMessage(level, message);
            }
        }

        protected abstract void Write(string message);
    }

    public class DebugLogger : AbstractLogger
    {
        public DebugLogger(int level)
        {
            Level = level;
        }
        protected override void Write(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }

    public class WarningLogger : AbstractLogger
    {
        public WarningLogger(int level)
        {
            Level = level;
        }
        protected override void Write(string message)
        {
            UnityEngine.Debug.LogWarning(message);
        }
    }

    public class ErrorLogger : AbstractLogger
    {
        public ErrorLogger(int level)
        {
            Level = level;
        }
        protected override void Write(string message)
        {
            UnityEngine.Debug.LogError(message);
        }
    }

    public static AbstractLogger GetChainOfLoggers()
    {
        AbstractLogger error = new DebugLogger(AbstractLogger.Error);
        AbstractLogger warning = new WarningLogger(AbstractLogger.Warning);
        AbstractLogger debug = new ErrorLogger(AbstractLogger.Debug);

        error.SetNextLogger(warning);
        warning.SetNextLogger(debug);

        return error;
    }


    public void Main()
    {
        AbstractLogger loggerChian = GetChainOfLoggers();
        loggerChian.LogMessage(AbstractLogger.Debug, "debug");
        loggerChian.LogMessage(AbstractLogger.Warning, "warning");
        loggerChian.LogMessage(AbstractLogger.Error, "error");
    }
}
