using System;

namespace Codestellation.Pulsar.Cron
{
    public class CronExpression
    {
        private SimpleCronField _seconds;
        private SimpleCronField _minutes;
        private SimpleCronField _hours;
        private DayOfMonthField _dayOfMonth;
        private SimpleCronField _month;
        private DayOfWeekField _dayOfWeek;
        private SimpleCronField _year;

        public CronExpression(string value)
        {
            var tokens = CronParser.Tokenize(value, CronSymbols.Space);

            if (tokens.Length < 6 || 7 < tokens.Length)
            {
                throw new FormatException("Expression must contain 6 or 7 space separated values.");
            }

            _seconds = SimpleCronField.ParseSeconds(tokens[0]);
            _minutes = SimpleCronField.ParseMinutes(tokens[1]);
            _hours = SimpleCronField.ParseHours(tokens[2]);

            _dayOfMonth = new DayOfMonthField(tokens[3]);
            _month = SimpleCronField.ParseMonth(tokens[4]);
            _dayOfWeek = new DayOfWeekField(tokens[5]);

            if (tokens.Length == 7)
            {
                _year = SimpleCronField.ParseYear(tokens[6]);
            }
        }

        public DateTime? NearestFrom(DateTime startingPoint)
        {
            return null;
        }
    }
}