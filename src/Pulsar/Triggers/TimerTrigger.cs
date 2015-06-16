using System;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    public abstract class TimerTrigger : ITrigger
    {
        private TriggerCallback _callback;
        private readonly ITimer _timer;

        public TimerTrigger(ITimer timer)
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

        public void Start(TriggerCallback callback)
        {
            if (callback == null)
            {
                throw new ArgumentNullException("callback");
            }
            _callback = callback;

            OnStart();
        }

        protected abstract void OnStart();

        public void Stop()
        {
            _timer.Stop();
            OnStop();
        }

        protected virtual void OnStop()
        {
        }

        protected void SetupTimer(DateTime fireAt, TimeSpan? interval = null)
        {
            _timer.Fire(fireAt, interval);
        }

        protected void InvokeCallback()
        {
            var context = new TriggerContext(this);
            _callback(context);
        }
    }
}