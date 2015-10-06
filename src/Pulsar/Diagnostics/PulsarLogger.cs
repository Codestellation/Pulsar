namespace Codestellation.Pulsar.Diagnostics
{
    /// <summary>
    /// Encapsulates log writing logic
    /// </summary>
    public class PulsarLogger
    {
        public bool IsDebugEnabled => LogSettings.LogToConsole;

        public static void Debug(string loggerName, string message)
        {
            if (LogSettings.LogToConsole)
            {
            }
        }
    }

    public static class PulsarLogManager
    {
    }
}