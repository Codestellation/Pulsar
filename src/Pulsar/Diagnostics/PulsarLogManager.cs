using System;
using System.Linq.Expressions;
using System.Reflection;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Diagnostics
{
    /// <summary>
    /// Manages logging for pulsar project
    /// </summary>
    public static class PulsarLogManager
    {
        /// <summary>
        /// Returns true if it possible to log console. Returns false otherwise.
        /// <remarks>It's not releated to <see cref="LogSettings.LogToConsole"/> property, but depends on <see cref="System.Console"/> class availability</remarks>
        /// </summary>
        public static readonly bool CanLogToConsole;

        private static readonly Action<string> ConsoleWriteLineDelegate;

        static PulsarLogManager()
        {
            var consoleType = Type.GetType("System.Console, mscorlib");
            if (consoleType == null)
            {
                CanLogToConsole = false;
                return;
            }
            CanLogToConsole = true;
            var writeLineMethod = consoleType.GetRuntimeMethod("WriteLine", new[] { typeof(string) });
            if (writeLineMethod == null)
            {
                throw new InvalidOperationException("Could not find WriteLine method");
            }

            var stringParameter = Expression.Parameter(typeof(string));
            var writeLineCall = Expression.Call(writeLineMethod, stringParameter);

            ConsoleWriteLineDelegate = Expression.Lambda<Action<string>>(writeLineCall, stringParameter).Compile();
        }

        /// <summary>
        /// Returns new instance of <see cref="PulsarLogger"/>
        /// </summary>
        /// <typeparam name="T">A type to create logger for</typeparam>
        public static PulsarLogger GetLogger<T>()
        {
            return new PulsarLogger(typeof(T).FullName);
        }

        internal static void ConsoleWriteLine(PulsarLogger pulsarLogger, string level, string message)
        {
            if (CanLogToConsole && LogSettings.LogToConsole)
            {
                ConsoleWriteLineDelegate($"{Clock.UtcNow.ToLocalTime().ToString(LogSettings.ConsoleDateFormat)} {pulsarLogger.ShortName} {level}: {message}");
            }
        }
    }
}