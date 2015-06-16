using System;
using Codestellation.Pulsar.Cron;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    public class CronTrigger : TimerTrigger
    {
        private readonly CronExpression _cronExpression;

        public DateTime? NextFireAt
        {
            get { return _cronExpression.NearestAfter(Clock.UtcNow); }
        }

        public CronTrigger(string cronExpression, ITimer timer)
            : this(new CronExpression(cronExpression), timer)
        {

        }

        public CronTrigger(CronExpression cronExpression, ITimer timer)
            : base(timer)
        {
            if (cronExpression == null)
            {
                throw new ArgumentNullException("cronExpression");
            }

            _cronExpression = cronExpression;
        }

        private void SetupTimer()
        {
            if (NextFireAt.HasValue)
            {
                SetupTimer(NextFireAt.Value);
            }
        }

        protected override void OnTimer()
        {
            SetupTimer();
            InvokeCallback();
        }

        protected override void OnStart()
        {
            SetupTimer();
        }
    }
}