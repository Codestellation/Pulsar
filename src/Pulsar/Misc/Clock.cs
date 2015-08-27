using System;

namespace Codestellation.Pulsar.Misc
{
    public static class Clock
    {
        private static readonly Func<DateTime> DefaultUtcNow;
        private static Func<DateTime> _utcNow = DefaultUtcNow;

        static Clock()
        {
            DefaultUtcNow = () => DateTime.UtcNow;
            UtcNowFunction = DefaultUtcNow;
        }

        public static DateTime UtcNow => _utcNow();

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

                _utcNow = value;
            }
        }

        public static void UseDefault()
        {
            UtcNowFunction = DefaultUtcNow;
        }
    }
}