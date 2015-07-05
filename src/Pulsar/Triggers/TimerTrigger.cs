using System;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    public abstract class TimerTrigger : AbstractTrigger
    {
        private readonly ITimer _timer;

        protected TimerTrigger(ITimer timer)
        {
            if (timer == null)
            {
                throw new ArgumentNullException("timer");
            }

            _timer = timer;
            _timer.OnFired += OnTimer;
        }

        protected virtual void OnTimer()
        {
            InvokeCallback();
        }

        protected override void OnStop()
        {
            _timer.Stop();
        }

        protected void SetupTimer(DateTime fireAt, TimeSpan? interval = null)
        {
            _timer.Fire(fireAt, interval);
        }
    }
}