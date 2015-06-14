using System;
using System.Threading;

namespace Codestellation.Pulsar.Timers
{
    public abstract class AbstractTimer : ITimer, IDisposable
    {
        public Func<DateTime> UtcNow = () => DateTime.UtcNow;

        private readonly Timer _internalTimer;
        
        public virtual event Action OnFired;

        protected AbstractTimer()
        {
            _internalTimer = new Timer(ignore => OnInternalTimerFired(), this, Timeout.Infinite, Timeout.Infinite);
        }

        public void Fire(DateTime startAt, TimeSpan? interval)
        {
            if (interval < TimeSpan.FromSeconds(0))
            {
                throw new ArgumentOutOfRangeException("interval", interval, "Value should be greater than or equal to zero.");
            }

            if (startAt.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException("DateTimeKind must be Local or Utc, but was Unspecified", "fireAt");
            }
            SetupInternal(startAt, interval);
        }

        protected abstract void SetupInternal(DateTime fireAt, TimeSpan? interval);

        protected void SetupInternalTimer(TimeSpan fireAt, TimeSpan interval)
        {
            _internalTimer.Change(fireAt, interval);
        }
        
        protected abstract void OnInternalTimerFired();


        protected void Callback()
        {
            var onFired = OnFired;
            Thread.MemoryBarrier();
            if (onFired != null)
            {
                onFired();
            }
        }

        public virtual void Dispose()
        {
            _internalTimer.Dispose();
        }
    }
}