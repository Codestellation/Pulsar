using System;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Diagnostics
{
    /// <summary>
    /// Contains sett
    /// </summary>
    public static class LogSettings
    {
        private static string _consoleDateFormat = "O";

        /// <summary>
        /// Write logs to console
        /// </summary>
        public static bool LogToConsole { get; set; }

        /// <summary>
        /// Gets or sets <see cref="DateTime"/> format to log to console
        /// </summary>
        public static string ConsoleDateFormat
        {
            get { return _consoleDateFormat; }
            set
            {
                //Ensure valid format
                Clock.UtcNow.ToString(value);
                _consoleDateFormat = value;
            }
        }
    }
}