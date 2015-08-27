using System;
using System.Threading.Tasks;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Timers
{
    public class PreciseTimer : AbstractTimer
    {
        private static readonly TimeSpan MaxTimerInterval = TimeSpan.FromMinutes(1);

        private DateTime _firstStartAt;
        private TimeSpan? _interval;
        private DateTime _nextStartat;

        protected override void SetupInternal(DateTime startAt, TimeSpan? interval)
        {
            _firstStartAt = startAt;
            _nextStartat = _firstStartAt;
            _interval = interval;

            TimeSpan fireAfter = CalculateFireAfter();

            SetupInternalTimer(fireAfter, TimeSpan.Zero);
        }

        protected override void OnInternalTimerFired()
        {
            var fireAfter = CalculateFireAfter();

            if (fireAfter <= TimeSpan.Zero)
            {
                Task.Run((Action)Callback);
                if (_interval.HasValue)
                {
                    _nextStartat += _interval.Value;
                    OnInternalTimerFired();
                }
                return;
            }

            SetupInternalTimer(fireAfter, TimeSpan.Zero);
        }

        private TimeSpan CalculateFireAfter()
        {
            var fireAfter = _nextStartat - Clock.UtcNow;

            if (fireAfter > MaxTimerInterval)
            {
                fireAfter = MaxTimerInterval;
            }

            if (fireAfter < TimeSpan.Zero)
            {
                fireAfter = TimeSpan.Zero;
            }
            return fireAfter;
        }
    }
}