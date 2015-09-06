using System;
using System.Threading;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// Base class for <see cref="ITimer"/> implementations
    /// </summary>
    public abstract class AbstractTimer : ITimer, IDisposable
    {
        private readonly Timer _internalTimer;

        /// <summary>
        /// Called when internal timer fires
        /// </summary>
        public virtual event Action OnFired;

        /// <summary>
        /// Initializes new instance of <see cref="AbstractTimer"/>
        /// </summary>
        protected AbstractTimer()
        {
            _internalTimer = new Timer(ignore => OnInternalTimerFired(), this, Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Forces timer to fire events with specifed parameters
        /// </summary>
        /// <param name="startAt">First time to fire. Must be UTC or Local</param>
        /// <param name="interval">Interval. Should positive or zero or null</param>
        public void Fire(DateTime startAt, TimeSpan? interval = null)
        {
            if (interval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Value should be greater than or equal to zero.");
            }

            if (startAt.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException("Datetime kind must not be " + nameof(DateTimeKind.Local) + " or " + nameof(DateTimeKind.Utc));
            }

            var startAtUtc = startAt.Kind == DateTimeKind.Local ? startAt.ToUniversalTime() : startAt;

            SetupInternal(startAtUtc, interval);
        }

        /// <summary>
        /// Forces timer to cease OnFired events.
        /// </summary>
        public void Stop()
        {
            _internalTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Template method for descendants setup
        /// </summary>
        protected abstract void SetupInternal(DateTime startAt, TimeSpan? interval);

        /// <summary>
        /// Setups internal timer with specified parameters
        /// </summary>
        protected void SetupInternalTimer(TimeSpan fireAfter, TimeSpan interval)
        {
            _internalTimer.Change(fireAfter, interval);
        }

        /// <summary>
        /// Called when internal timer fires
        /// </summary>
        protected abstract void OnInternalTimerFired();

        /// <summary>
        /// Raises <see cref="OnFired"/> event
        /// </summary>
        protected void RaiseOnFired()
        {
            Volatile.Read(ref OnFired)?.Invoke();
        }

        /// <summary>
        /// Disposes internal timer
        /// </summary>
        public virtual void Dispose()
        {
            _internalTimer.Dispose();
        }
    }
}