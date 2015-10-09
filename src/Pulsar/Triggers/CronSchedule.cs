using System;
using Codestellation.Pulsar.Cron;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Calculates next occurrence using <see cref="CronExpression"/>
    /// </summary>
    public class CronSchedule : ISchedule
    {
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZone;

        /// <summary>
        /// Initialize new instance of <see cref="CronSchedule"/>
        /// </summary>
        /// <param name="cronExpression">An expression to calculate schedule on</param>
        /// <param name="timeZone">Time zone of the schedule</param>
        /// <exception cref="ArgumentNullException"></exception>
        public CronSchedule(CronExpression cronExpression, TimeZoneInfo timeZone)
        {
            if (cronExpression == null)
            {
                throw new ArgumentNullException(nameof(cronExpression));
            }

            if (timeZone == null)
            {
                throw new ArgumentNullException(nameof(timeZone));
            }
            _cronExpression = cronExpression;
            _timeZone = timeZone;
        }

        /// <summary>
        /// Returns next datetime if possible
        /// <remarks>Should return <see cref="DateTimeKind.Utc"/> <see cref="DateTime"/></remarks>
        /// </summary>
        public DateTime? NextAt
        {
            get
            {
                DateTime utcNow = Clock.UtcNow;
                DateTime zoneNow = TimeZoneInfo.ConvertTime(utcNow, _timeZone);

                DateTime? zoneNearest = _cronExpression.NearestAfter(zoneNow);
                if (!zoneNearest.HasValue)
                {
                    return null;
                }
                var offset = _timeZone.GetUtcOffset(zoneNearest.Value);
                var nextAt = new DateTime(zoneNearest.Value.Ticks, DateTimeKind.Utc).Add(-offset);
                return nextAt;
            }
        }
    }
}