using System;

namespace Codestellation.Pulsar.Cron
{
    public class DayOfWeekField : SimpleCronField
    {
        private bool _notSpecified;
        private const int Sunday = 1;
        private const int Saturday = 7;

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

        public DayOfWeekField(string token) : base(token, Sunday, Saturday)
        {
            if (CronParser.IsNotSpecifed(token))
            {
                NotSpecified = true;
            }
        }

        private bool IsLastDay(DateTime date)
        {
            return date.DayOfWeek == DayOfWeek.Saturday;
        }

        protected override void ParseToken(string token)
        {
            if (CronParser.IsNotSpecifed(token))
            {
                NotSpecified = true;
                return;
            }
            if (CronParser.IsNumberedWeekday(token))
            {
                AddSelector(CronParser.ParseNumberedWeekday(token));
                return;
            }
            if (CronParser.IsLast(token))
            {
                AddSelector(IsLastDay);
                return;
            }
            base.ParseToken(token);
        }
    }
}