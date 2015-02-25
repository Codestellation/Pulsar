using System;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfMonthField : SimpleCronField
    {
        public const int MinDay = 1;
        public const int MaxDay = 31;

        private bool _notSpecified;

        public bool NotSpecified
        {
            get { return _notSpecified; }
            private set
            {
                _notSpecified = value;
                ClearValues();
                ClearSelectors();
            }
        }

        public DayOfMonthField(string token) : base(token, CronFieldSettings.DayOfMonth)
        {
            if (CronParser.IsNotSpecifed(token))
            {
                NotSpecified = true;
            }
        }

        protected override void ParseToken(string token)
        {
            if (CronParser.IsLast(token))
            {
                AddSelector(LastDayOfMonth);
                return;
            }
            if (CronParser.IsWeekday(token))
            {
                AddSelector(Weekday);
                return;
            }
            if (CronParser.IsLastWeekday(token))
            {
                AddSelector(LastWeekday);
                return;
            }
            if (CronParser.IsNotSpecifed(token))
            {
                return;
            }
            base.ParseToken(token);
        }

        private static bool Weekday(DateTime point)
        {
            switch (point.DayOfWeek)
            {
                default:
                    return true;
                case DayOfWeek.Saturday:
                case DayOfWeek.Sunday:
                    return false;
            }
        }

        private static bool LastDayOfMonth(DateTime point)
        {
            return point.Day ==  CronDateHelper.GetLastDayOfMonth(point);
        }

        private static bool LastWeekday( DateTime point)
        {
            var lastDay = CronDateHelper.GetLastDayOfMonth(point.Year, point.Month);
            var candidate = new DateTime(point.Year, point.Month, lastDay);

            while (!Weekday(candidate))
            {
                candidate = candidate.AddDays(-1);
            }

            return point.Day == candidate.Day;
        }
    }
}