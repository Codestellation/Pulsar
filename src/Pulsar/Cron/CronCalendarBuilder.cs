using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar.Cron
{
    public class CronCalendarBuilder
    {
        private SimpleCronField _month;
        private DayOfMonthField _dayOfMonth;
        private DayOfWeekField _dayOfWeek;

        public CronCalendarBuilder SetMonth(SimpleCronField month)
        {
            _month = month;
            return this;
        }

        public CronCalendarBuilder SetDayOfMonth(DayOfMonthField dayOfMonth)
        {
            _dayOfMonth = dayOfMonth;
            return this;
        }

        public CronCalendarBuilder SetDayOfWeek(DayOfWeekField dayOfWeek)
        {
            _dayOfWeek = dayOfWeek;
            return this;
        }

        public CronCalendar BuildFor(int year)
        {
            if (year <= 2000 && 2050 <= year)
            {
                throw new ArgumentOutOfRangeException("year", year, "Year should be bewteen 2000 and 2050");
            }

            Validate();



            var scheduledDays = new List<DateTime>();

            var startPoint = new DateTime(year, 1, 1);
            for (int monthIndex = 0; monthIndex < 12; monthIndex++)
            {
                var currentPoint = startPoint.AddMonths(monthIndex);

                if (_month.ShouldFire(currentPoint))
                {
                    var lastDayOfMonth = CronDateHelper.GetLastDayOfMonth(currentPoint);

                    for (int dayIndex = 0; dayIndex < lastDayOfMonth; dayIndex++)
                    {
                        if (_dayOfMonth.ShouldFire(currentPoint))
                        {
                            scheduledDays.Add(currentPoint);
                        }
                        currentPoint = currentPoint.AddDays(1);
                    }
                }
            }

            return new CronCalendar(scheduledDays);
        }

        private void Validate()
        {
            if (_month == null)
            {
                throw new InvalidOperationException("Please set month before build");
            }

            if (_dayOfMonth == null)
            {
                throw new InvalidOperationException("Please set day of month before build");
            }
            
            if (_dayOfWeek == null)
            {
                throw new InvalidOperationException("Please set day of week before build");
            }

            if (!_dayOfWeek.NotSpecified && !_dayOfMonth.NotSpecified)
            {
                throw new InvalidOperationException("Day of month and day of week must not be specified together. Please, set one of them to not specified value '?'");
            }
        }
    }
}