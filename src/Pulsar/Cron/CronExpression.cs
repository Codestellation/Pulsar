using System;

namespace Codestellation.Pulsar.Cron
{
    public class CronExpression
    {
        private TimeField _seconds;
        private TimeField _minutes;
        private TimeField _hours;
        private DayOfMonthField _dayOfMonth;

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

        }

        public DateTime? NearestFrom(DateTime startingPoint)
        {
            var nearestHour = _hours.Nearest(startingPoint.Hour);
            var nearestMinute = _minutes.Nearest(startingPoint.Minute);
            var nearestSecond = _seconds.Nearest(startingPoint.Second);


            return new DateTime(startingPoint.Year, startingPoint.Month, startingPoint.Day, nearestHour, nearestMinute, nearestSecond);
        }
    }
}