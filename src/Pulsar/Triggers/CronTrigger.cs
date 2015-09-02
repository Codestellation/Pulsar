using System;
using Codestellation.Pulsar.Cron;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Fires on schedule specified in cron expression
    /// </summary>
    public class CronTrigger : TimerTrigger
    {
        private readonly CronExpression _cronExpression;

        /// <summary>
        /// Returns nearest timer to fire
        /// </summary>
        public DateTime? NextFireAt => _cronExpression.NearestAfter(Clock.UtcNow);

        /// <summary>
        /// Initialize new instance of <see cref="CronTrigger"/>
        /// </summary>
        /// <param name="cronExpression">Should be valid cron expression</param>
        /// <param name="timer"></param>
        public CronTrigger(string cronExpression, ITimer timer)
            : this(new CronExpression(cronExpression), timer)
        {
        }

        /// <summary>
        /// Initialize new instance of <see cref="CronTrigger"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        public CronTrigger(CronExpression cronExpression, ITimer timer)
            : base(timer)
        {
            if (cronExpression == null)
            {
                throw new ArgumentNullException(nameof(cronExpression));
            }

            _cronExpression = cronExpression;
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