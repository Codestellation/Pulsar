using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class CronCalendar
    {
        private readonly List<DateTime> _days;

        public CronCalendar(IEnumerable<DateTime> scheduledDays)
        {
            _days = scheduledDays.ToList();
            _days.Sort();
        }

        public IEnumerable<DateTime> ScheduledDays
        {
            get { return _days; }
        }

        public IEnumerable<DateTime> DaysAfter(DateTime point)
        {
            var date = point.Date;
            for (int dayIndex = 0; dayIndex < _days.Count; dayIndex++)
            {
                var candidate = _days[dayIndex];

                if (date <= candidate)
                {
                    yield return candidate;
                }

            }
        }
    }
}