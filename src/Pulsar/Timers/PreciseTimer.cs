using System;
using System.Threading;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Timers
{
    public class PreciseTimer : AbstractTimer
    {
        private static readonly TimeSpan MaxTime = TimeSpan.FromMinutes(1);

        private DateTime _startAt;
        private TimeSpan? _interval;
        
        private bool _shouldFireCallback;

        protected override void SetupInternal(DateTime startAt, TimeSpan? interval)
        {
            _interval = interval;
            _startAt = startAt.ToUniversalTime();
            _shouldFireCallback = false;
            SetupTimer(false);
        }

        private void SetupTimer(bool isTimerCallback)
        {
            var fireAfter = _startAt - Clock.UtcNow;

            if (fireAfter <= TimeSpan.Zero) 
            {
                if (isTimerCallback)
                {
                    Callback();
                }
                else
                {
                    ThreadPool.UnsafeQueueUserWorkItem(state => Callback(), null);
                }

                SetupFireSinceInterval();
            }
            else if (fireAfter > MaxTime)
            {
                SetupInternalTimer(MaxTime, TimeSpan.Zero);
            }
            else
            {
                _shouldFireCallback = true;
                SetupInternalTimer(fireAfter, TimeSpan.Zero);
            }
        }

        protected override void OnInternalTimerFired()
        {
            if (_shouldFireCallback)
            {
                Callback();
                SetupFireSinceInterval();
            }
            else
            {
                SetupTimer(true);
            }
        }

        private void SetupFireSinceInterval()
        {
            if (_interval.HasValue)
            {
                _startAt = _startAt + _interval.Value;
                SetupTimer(true);
            }
        }
    }
}