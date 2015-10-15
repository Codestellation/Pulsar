using System;
using System.Threading;
using Codestellation.Pulsar.Diagnostics;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// Base class for <see cref="ITimer"/> implementations
    /// </summary>
    public abstract class AbstractTimer : ITimer, IDisposable
    {
        private static readonly PulsarLogger Logger = PulsarLogManager.GetLogger<AbstractTimer>();

        private readonly Timer _internalTimer;
        private bool _disposed;

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
            EnsureNotDisposed();
            if (interval < TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), interval, "Value should be greater than or equal to zero.");
            }

            if (startAt.Kind == DateTimeKind.Unspecified)
            {
                throw new ArgumentException("Datetime kind must not be " + nameof(DateTimeKind.Local) + " or " + nameof(DateTimeKind.Utc));
            }

            var startAtUtc = startAt.Kind == DateTimeKind.Local ? startAt.ToUniversalTime() : startAt;

            if (Logger.IsInfoEnabled)
            {
                Logger.Info($"Set up timer at {startAt} with {interval.ToString() ?? "no"} interval");
            }

            SetupInternal(startAtUtc, interval);
        }

        /// <summary>
        /// Forces timer to cease OnFired events.
        /// </summary>
        public void Stop()
        {
            if (!_disposed)
            {
                _internalTimer.Change(Timeout.Infinite, Timeout.Infinite);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug($"Stopped");
                }
            }
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
            if (!_disposed)
            {
                _internalTimer.Change(fireAfter, interval);

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug($"Set up internal: fireAfter =  {fireAfter},  interval = {interval}");
                }
            }
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
            if (!_disposed)
            {
                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Fired");
                }

                Volatile.Read(ref OnFired)?.Invoke();
            }
        }

        /// <summary>
        /// Disposes internal timer
        /// </summary>
        public virtual void Dispose()
        {
            if (!_disposed)
            {
                _disposed = true;
                _internalTimer.Dispose();

                if (Logger.IsDebugEnabled)
                {
                    Logger.Debug("Disposed");
                }
            }
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException("Timer disposed");
            }
        }
    }
}