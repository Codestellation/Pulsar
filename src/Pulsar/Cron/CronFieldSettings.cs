using System;

namespace Codestellation.Pulsar.Cron
{
    public struct CronFieldSettings
    {
        public readonly int MinValue;
        public readonly int MaxValue;
        public readonly Func<DateTime, int> DatePartSelector;

        private CronFieldSettings(int minValue, int maxValue, Func<DateTime, int> datePartSelector)
        {
            MinValue = minValue;
            MaxValue = maxValue;
            DatePartSelector = datePartSelector;
        }

        public static CronFieldSettings DayOfMonth => new CronFieldSettings(1, 31, x => x.Day);

        public static CronFieldSettings Second => new CronFieldSettings(0, 59, x => x.Second);

        public static CronFieldSettings Minute => new CronFieldSettings(0, 59, x => x.Minute);

        public static CronFieldSettings Hour => new CronFieldSettings(0, 23, x => x.Hour);

        public static CronFieldSettings Month => new CronFieldSettings(1, 12, x => x.Month);

        public static CronFieldSettings Year => new CronFieldSettings(2000, 2100, x => x.Year);

        public static CronFieldSettings DayOfWeed => new CronFieldSettings(1, 7, x => CronDateHelper.ToCronValue(x.DayOfWeek));
    }
}