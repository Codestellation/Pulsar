using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar.Cron
{
    public class CronCalendarBuilder
    {
        private TimeField _month;
        private DayOfMonthField _dayOfMonth;

        public CronCalendarBuilder SetMonth(TimeField month)
        {
            _month = month;
            return this;
        }

        public CronCalendarBuilder SetDayOfMonth(DayOfMonthField dayOfMonth)
        {
            _dayOfMonth = dayOfMonth;
            return this;
        }

        public CronCalendar BuildFor(int year)
        {
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
    }
}