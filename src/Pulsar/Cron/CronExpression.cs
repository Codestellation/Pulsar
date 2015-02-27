using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar.Cron
{
    public class CronExpression
    {
        private SimpleCronField _year;
        private CronCalendarBuilder _builder;
        private CronDaySchedule _daySchedule;
        private Dictionary<int, CronCalendar> _calendars;
        private int _minYear;
        private int _maxYear;

        public CronExpression(string value)
        {
            var tokens = CronParser.Tokenize(value, CronSymbols.Space);

            if (tokens.Length < 6 || 7 < tokens.Length)
            {
                throw new FormatException("Expression must contain 6 or 7 space separated values.");
            }

            _calendars = new Dictionary<int, CronCalendar>();

            var seconds = SimpleCronField.ParseSeconds(tokens[0]);
            var minutes = SimpleCronField.ParseMinutes(tokens[1]);
            var hours = SimpleCronField.ParseHours(tokens[2]);
            
            _daySchedule = new CronDaySchedule(seconds, minutes, hours);


            var dayOfMonth = new DayOfMonthField(tokens[3]);
            var month = SimpleCronField.ParseMonth(tokens[4]);
            var dayOfWeek = new DayOfWeekField(tokens[5]);

            _builder = new CronCalendarBuilder()
                .SetMonth(month)
                .SetDayOfWeek(dayOfWeek)
                .SetDayOfMonth(dayOfMonth);

            if (tokens.Length == 7)
            {
                _year = SimpleCronField.ParseYear(tokens[6]);
                _minYear = _year.MinValue;
                _maxYear = _year.MaxValue;
            }
            else
            {
                _minYear = 2000;
                _maxYear = 2100;
            }
        }

        public DateTime? NearestAfter(DateTime point)
        {
            var currentPoint = point;
            var milliseconds = point.Millisecond;
            if (milliseconds > 0)
            {
                currentPoint = currentPoint.AddMilliseconds(1000 - milliseconds);
            }
            
            CronCalendar calendar;
            while (TryGetCalendar(currentPoint, out calendar))
            {
                var currentPointDate = currentPoint.Date;
                
                if (calendar.ShouldFire(currentPointDate))
                {
                    TimeSpan fireAt;
                    if(_daySchedule.TryGetTimeAfter(currentPoint.TimeOfDay, out fireAt))
                    {
                        return currentPointDate.Add(fireAt);
                    }
                }
                DateTime closest;
                if (calendar.TryFindNextDay(currentPointDate, out closest))
                {
                    var time = _daySchedule.MinTime;
                    return closest.Add(time);
                }

                currentPoint = CronDateHelper.BeginOfNextYear(currentPoint);
            }
            return null;
        }

        private bool TryGetCalendar(DateTime point, out CronCalendar calendar)
        {
            var year = point.Year;

            if (year < _minYear && _maxYear < year)
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