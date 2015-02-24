using System;

namespace Codestellation.Pulsar.Cron
{
    public class CronExpression
    {
        private TimeField _seconds;
        private TimeField _minutes;
        private TimeField _hours;
        private DayOfMonthField _dayOfMonth;
        private TimeField _month;
        private DayOfWeekField _dayOfWeek;

        public CronExpression(string value)
        {
            var tokens = value.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (tokens.Length < 6 || 7 < tokens.Length)
            {
                throw new FormatException("Expression must contain 6 or 7 space separated values.");
            }

            _seconds = TimeField.ParseSeconds(tokens[0]);
            _minutes = TimeField.ParseMinutes(tokens[1]);
            _hours = TimeField.ParseHours(tokens[2]);

            _dayOfMonth = DayOfMonthField.Parse(tokens[3]);
            _month = TimeField.ParseMonth(tokens[4]);
            _dayOfWeek = new DayOfWeekField(tokens[5]);
        }

        public DateTime? NearestFrom(DateTime startingPoint)
        {
            return null;
        }
    }
}