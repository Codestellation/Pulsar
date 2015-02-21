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

        public CronExpression(string value)
        {
            var tokens = value.Split(' ');

            if (tokens.Length < 6 || 7 < tokens.Length)
            {
                throw new FormatException("Expression must contain 6 or 7 space separated values.");
            }

            _seconds = TimeField.ParseSeconds(tokens[0]);
            _minutes = TimeField.ParseMinutes(tokens[1]);
            _hours = TimeField.ParseHours(tokens[2]);

            _dayOfMonth = DayOfMonthField.Parse(tokens[3]);
            _month = TimeField.ParseMonth(tokens[4]);

        }

        public DateTime? NearestFrom(DateTime startingPoint)
        {
            var candidate = startingPoint.Date;
            
            int year = startingPoint.Year;
            int month = _month.GetClosestTo(candidate);
            int day = _dayOfMonth.GetClosestTo(candidate);

            int hour = _hours.Nearest(startingPoint.Hour);
            int minute = _minutes.Nearest(startingPoint.Minute);
            int second = _seconds.Nearest(startingPoint.Second);


            return new DateTime(year, month, day, hour, minute, second);
        }
    }
}