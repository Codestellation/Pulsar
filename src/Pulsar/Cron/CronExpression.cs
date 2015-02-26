using System;
using System.Collections.Generic;

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
        private CronCalendarBuilder _builder;
        private CronDaySchedule _daySchedule;
        private Dictionary<int, CronCalendar> _calendars;

        public CronExpression(string value)
        {
            var tokens = CronParser.Tokenize(value, CronSymbols.Space);

            if (tokens.Length < 6 || 7 < tokens.Length)
            {
                throw new FormatException("Expression must contain 6 or 7 space separated values.");
            }

            _calendars = new Dictionary<int, CronCalendar>();

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

            _builder = new CronCalendarBuilder()
                .SetMonth(_month)
                .SetDayOfWeek(_dayOfWeek)
                .SetDayOfMonth(_dayOfMonth);

            _daySchedule = new CronDaySchedule(_seconds, _minutes, _hours);
        }

        public DateTime? NearestAfter(DateTime point)
        {
            var currentPoint = point;
            CronCalendar calendar;
            while (TryGetCalendar(currentPoint, out calendar))
            {
                foreach (var date in calendar.DaysAfter(point))
                {
                    foreach (var time in _daySchedule.Values)
                    {
                        var candidate = date.Add(time);
                        if (candidate >= point)
                        {
                            return candidate;
                        }
                    }
                }
                currentPoint = CronDateHelper.BeginOfNextYear(currentPoint);
            }
            return null;
        }

        private bool TryGetCalendar(DateTime point, out CronCalendar calendar)
        {
            var year = point.Year;
            if (2100 < year)
            {
                calendar = null;
                return false;
            }

            if (!_calendars.TryGetValue(year, out calendar))
            {
                calendar = _builder.BuildFor(year);
                _calendars[year] = calendar;
            }
            return true;
        }
    }
}