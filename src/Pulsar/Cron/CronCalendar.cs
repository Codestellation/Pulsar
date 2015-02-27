using System;
using System.Collections.Generic;
using System.Linq;

namespace Codestellation.Pulsar.Cron
{
    public class CronCalendar
    {
        private readonly List<DateTime> _days;
        private readonly HashSet<DateTime> _dayIndex;

        public CronCalendar(IEnumerable<DateTime> scheduledDays)
        {
            _days = scheduledDays.ToList();
            _days.Sort();
            _dayIndex = new HashSet<DateTime>(_days);
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

        public bool ShouldFire(DateTime date)
        {
            return _dayIndex.Contains(date);
        }

        public bool TryFindNextDay(DateTime date, out DateTime closest)
        {
            for (int dayIndex = 0; dayIndex < _days.Count; dayIndex++)
            {
                var candidate = _days[dayIndex];

                if (date < candidate)
                {
                    closest = candidate;
                    return true;
                }
            }

            closest = DateTime.MinValue;
            return false;
        }
    }
}