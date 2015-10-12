using System;

namespace Codestellation.Pulsar.Diagnostics
{
    /// <summary>
    /// Encapsulates log writing logic
    /// </summary>
    public class PulsarLogger
    {
        /// <summary>
        /// Returns full name of a logger. Typically it equals to full name of a type logger created for.
        /// </summary>
        public string FullName { get; }

        /// <summary>
        /// Returns short name of logger. Typically it equals to the name of a class without namespace
        /// </summary>
        public string ShortName { get; }

        /// <summary>
        /// Gets it a debug message should be written
        /// </summary>
        public bool IsDebugEnabled { get; private set; }

        /// <summary>
        /// Gets it a informational message should be written
        /// </summary>
        public bool IsInfoEnabled { get; private set; }

        /// <summary>
        /// Gets it a warning message should be written
        /// </summary>
        public bool IsWarnEnabled { get; private set; }

        /// <summary>
        /// Initialize new instance of <see cref="PulsarLogger"/>
        /// </summary>
        /// <param name="fullName"></param>
        public PulsarLogger(string fullName)
        {
            if (string.IsNullOrWhiteSpace(fullName))
            {
                throw new ArgumentException("Must be neither null nor empty string");
            }
            FullName = fullName;
            int lastDotIndex = FullName.LastIndexOf('.');

            ShortName = lastDotIndex == -1
                ? FullName
                : FullName.Substring(lastDotIndex + 1);
        }

        /// <summary>
        /// Writes a message with of debug level
        /// </summary>
        public void Debug(string message)
        {
            PulsarLogManager.ConsoleWriteLine(this, "DEBUG", message);
        }

        /// <summary>
        /// Writes a message with of information level
        /// </summary>
        public void Info(string message)
        {
            PulsarLogManager.ConsoleWriteLine(this, "INFO", message);
        }

        /// <summary>
        /// Writes a message with of warning level
        /// </summary>
        public void Warn(string message)
        {
            PulsarLogManager.ConsoleWriteLine(this, "WARN", message);
        }
    }
}