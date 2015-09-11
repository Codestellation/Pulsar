using System;
using System.Diagnostics;
using Codestellation.Pulsar.Cron;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Fires on schedule specified in cron expression
    /// </summary>
    [DebuggerDisplay("{_cronExpression} Next = {NextFireAt}")]
    public class CronTrigger : TimerTrigger
    {
        private readonly CronExpression _cronExpression;
        private readonly TimeZoneInfo _timeZone;

        /// <summary>
        /// Returns nearest timer to fire
        /// </summary>
        public DateTime? NextFireAt
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

                TimeSpan offset = _timeZone.GetUtcOffset(zoneNearest.Value);
                DateTime nextFireAt = zoneNearest.Value - offset;
                return nextFireAt;
            }
        }

        /// <summary>
        /// Initialize new instance of <see cref="CronTrigger"/>
        /// </summary>
        /// <param name="cronExpression">Should be valid cron expression</param>
        /// <param name="timeZone">Forces cron trigger to fire on time in specified  <see cref="TimeZoneInfo"/></param>
        /// <param name="timer">Timer implementation
        /// <remarks>It's recommended to use <see cref="PreciseTimer"/> for long intervals to avoid timer drift issues</remarks>
        /// </param>
        public CronTrigger(string cronExpression, TimeZoneInfo timeZone, ITimer timer)
            : this(new CronExpression(cronExpression), timeZone, timer)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="CronTrigger"/>
        /// </summary>
        /// <param name="cronExpression">Should be valid cron expression</param>
        /// <param name="timeZone">Forces cron trigger to fire on time in specified  <see cref="TimeZoneInfo"/></param>
        /// <param name="timer">Timer implementation
        /// <remarks>It's recommended to use <see cref="PreciseTimer"/> for long intervals to avoid timer drift issues</remarks>
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public CronTrigger(CronExpression cronExpression, TimeZoneInfo timeZone, ITimer timer)
            : base(timer)
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
        /// Invokes callback and restarts internal timer
        /// </summary>
        protected override void OnTimer()
        {
            OnStart();
            InvokeCallback();
        }

        /// <summary>
        /// Starts timer if it's possible due to cron expression
        /// </summary>
        protected override void OnStart()
        {
            if (NextFireAt.HasValue)
            {
                SetupTimer(NextFireAt.Value);
            }
        }
    }
}