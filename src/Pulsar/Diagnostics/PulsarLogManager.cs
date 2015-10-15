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
        /// <remarks>It's not releated to <see cref="LogSettings.LogToConsole"/> property, but depends on System.Console class availability</remarks>
        /// </summary>
        public static readonly bool CanLogToConsole;

        /// <summary>
        /// Gets or sets logger factory delegate.
        /// <remarks></remarks>
        /// </summary>
        public static Func<string, PulsarLogger> LoggerFactory
        {
            get { return _loggerFactory; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }
                _loggerFactory = value;
            }
        }

        private static readonly Action<string> ConsoleWriteLineDelegate;
        private static Func<string, PulsarLogger> _loggerFactory;

        static PulsarLogManager()
        {
            LoggerFactory = fullname => new PulsarLogger(fullname);
            ConsoleWriteLineDelegate = GenerateLogToConsoleDelegate();
            CanLogToConsole = ConsoleWriteLineDelegate != null;
        }

        private static Action<string> GenerateLogToConsoleDelegate()
        {
            var consoleType = Type.GetType("System.Console, mscorlib");

            var writeLineMethod = consoleType?.GetRuntimeMethod("WriteLine", new[] { typeof(string) });
            if (writeLineMethod == null)
            {
                return null;
            }

            var stringParameter = Expression.Parameter(typeof(string));
            var writeLineCall = Expression.Call(writeLineMethod, stringParameter);

            return Expression.Lambda<Action<string>>(writeLineCall, stringParameter).Compile();
        }

        /// <summary>
        /// Returns new instance of <see cref="PulsarLogger"/>
        /// </summary>
        /// <typeparam name="T">A type to create logger for</typeparam>
        public static PulsarLogger GetLogger<T>()
        {
            return GetLogger(typeof(T));
        }

        /// <summary>
        /// Returns new instance of <see cref="PulsarLogger"/>
        /// </summary>
        public static PulsarLogger GetLogger(Type type)
        {
            return LoggerFactory(type.FullName);
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