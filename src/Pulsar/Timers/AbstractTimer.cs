using System;
using System.Threading;

namespace Codestellation.Pulsar.Timers
{
    public abstract class AbstractTimer : ITimer, IDisposable
    {
        private readonly Timer _internalTimer;

        public virtual event Action OnFired;

        protected AbstractTimer()
        {
            _internalTimer = new Timer(ignore => OnInternalTimerFired(), this, Timeout.Infinite, Timeout.Infinite);
        }

        public void Fire(DateTime startAt, TimeSpan? interval = null)
        {
            if (interval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Value should be greater than or equal to zero.");
            }

            SetupInternal(startAt, interval);
        }

        public void Stop()
        {
            _internalTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        protected abstract void SetupInternal(DateTime startAt, TimeSpan? interval);

        protected void SetupInternalTimer(TimeSpan fireAt, TimeSpan interval)
        {
            _internalTimer.Change(fireAt, interval);
        }

        protected abstract void OnInternalTimerFired();

        protected void Callback()
        {
            Volatile.Read(ref OnFired)?.Invoke();
        }

        public virtual void Dispose()
        {
            _internalTimer.Dispose();
        }
    }
}