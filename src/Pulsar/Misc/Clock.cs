using System;
using Codestellation.Pulsar.Diagnostics;

namespace Codestellation.Pulsar.Misc
{
    /// <summary>
    /// Used internally by timers and triggers to define moment to fire. May be used for test purposes. 
    /// </summary>
    public static class Clock
    {
        private static readonly PulsarLogger Logger = PulsarLogManager.GetLogger(typeof(Clock));
        private static readonly Func<DateTime> DefaultUtcNow;
        private static Func<DateTime> _utcNow = DefaultUtcNow;

        static Clock()
        {
            DefaultUtcNow = () => DateTime.UtcNow;
            UtcNowFunction = DefaultUtcNow;
        }

        /// <summary>
        /// Returns current UTC <see cref="DateTime"/>
        /// </summary>
        public static DateTime UtcNow => _utcNow();

        /// <summary>
        /// Gets or sets delegate to generate <see cref="UtcNow"/>. 
        /// </summary>
        public static Func<DateTime> UtcNowFunction
        {
            get { return _utcNow; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                var probe = value();

                if (probe.Kind != DateTimeKind.Utc)
                {
                    throw new ArgumentException("Delegate must return UTC date time", nameof(value));
                }

                Logger.Info("Clock function changed");

                _utcNow = value;
            }
        }

        /// <summary>
        /// Restores default UtcNow functions <see cref="DateTime.UtcNow"/>
        /// </summary>
        public static void UseDefault()
        {
            UtcNowFunction = DefaultUtcNow;
        }
    }
}