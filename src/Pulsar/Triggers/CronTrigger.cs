using System;
using Codestellation.Pulsar.Cron;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    public class CronTrigger : ITrigger
    {
        private TriggerCallback _callback;
        private readonly CronExpression _cronExpression;
        private readonly ITimer _timer;

        public CronTrigger(string cronExpression, ITimer timer) : this(new CronExpression(cronExpression), timer)
        {
            
        }

        public CronTrigger(CronExpression cronExpression, ITimer timer)
        {
            if (cronExpression == null)
            {
                throw new ArgumentNullException("cronExpression");
            }

            if (timer == null)
            {
                throw new ArgumentNullException("timer");
            }

            _cronExpression = cronExpression;
            _timer = timer;
            _timer.OnFired += OnTimer;
        }

        public DateTime? NextFireAt
        {
            get { return _cronExpression.NearestAfter(Clock.UtcNow); }
        }

        public void Start(TriggerCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            _callback = callback;

            SetupTimer();
        }

        private void SetupTimer()
        {
            if (NextFireAt.HasValue)
            {
                _timer.Fire(NextFireAt.Value);
            }
        }

        public void Stop()
        {
            _timer.Stop();
        }

        private void OnTimer()
        {
            SetupTimer();
            var context = new TriggerContext(this);
            _callback(context);
        }
    }
}