using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// Similar to <see cref="SimpleTimer"/> but has reduced drift effect.
    /// <remarks>http://stackoverflow.com/questions/8431995/c-sharp-net-2-threading-timer-time-drifting</remarks>
    /// </summary>
    [DebuggerDisplay("Next = {_nextStartAt}")]
    public class PreciseTimer : AbstractTimer
    {
        private static readonly TimeSpan MaxTimerInterval = TimeSpan.FromMinutes(1);

        private DateTime _firstStartAt;
        private TimeSpan? _interval;
        private DateTime _nextStartAt;

        /// <summary>
        /// Sets next timer to fire from for internal timer
        /// </summary>
        /// <param name="startAt">First time to start at</param>
        /// <param name="interval">Repeat interval</param>
        protected override void SetupInternal(DateTime startAt, TimeSpan? interval)
        {
            _firstStartAt = startAt;
            _nextStartAt = _firstStartAt;
            _interval = interval;

            TimeSpan fireAfter = CalculateFireAfter();

            SetupInternalTimer(fireAfter, TimeSpan.Zero);
        }

        /// <summary>
        /// Called when internal timer fires.
        /// </summary>
        protected override void OnInternalTimerFired()
        {
            var fireAfter = CalculateFireAfter();

            if (fireAfter <= TimeSpan.Zero)
            {
                Task.Run((Action)RaiseOnFired);
                if (_interval.HasValue)
                {
                    _nextStartAt += _interval.Value;
                    OnInternalTimerFired();
                }
                return;
            }

            SetupInternalTimer(fireAfter, TimeSpan.Zero);
        }

        private TimeSpan CalculateFireAfter()
        {
            var fireAfter = _nextStartAt - Clock.UtcNow;

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