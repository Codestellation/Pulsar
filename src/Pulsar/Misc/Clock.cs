using System;

namespace Codestellation.Pulsar.Misc
{
    public static class Clock
    {
        private static readonly Func<DateTime> DefaultUtcNow = () => DateTime.Now;
        private static Func<DateTime> _utcNow = DefaultUtcNow;

        public static DateTime UtcNow
        {
            get { return _utcNow(); }
        }

        public static Func<DateTime> UtcNowFunction
        {
            get { return _utcNow; }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                var probe = value();

                if (probe.Kind != DateTimeKind.Utc)
                {
                    throw new ArgumentException("Delegate must return UTC date time", "value");
                }

                _utcNow = value;
            }
        }
    }
}