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
        }

        public IEnumerable<DateTime> ScheduledDays
        {
            get { return _days; }
        }
    }
}