using System;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    public class SimpleTimerTrigger : TimerTrigger
    {
        private readonly DateTime _startAt;
        private readonly TimeSpan? _interval;

        public SimpleTimerTrigger(DateTime startAt, TimeSpan? interval,  ITimer timer) : base(timer)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException("interval", "Must be greater than zero");
            }
            _startAt = startAt;
            _interval = interval;
        }

        protected override void OnStart()
        {
            SetupTimer(_startAt, _interval);
        }
    }
}