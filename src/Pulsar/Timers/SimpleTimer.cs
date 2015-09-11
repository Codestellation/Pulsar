using System;
using System.Diagnostics;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// Simple wrap over <see cref="System.Threading.Timer"/>
    /// </summary>
    [DebuggerDisplay("StartAt = {_startAt} Interval = {_interval}")]
    public class SimpleTimer : AbstractTimer
    {
        private DateTime _startAt;
        private TimeSpan? _interval;

        /// <summary>
        /// Performs initialization of timer
        /// </summary>
        /// <param name="startAt"></param>
        /// <param name="interval"></param>
        protected override void SetupInternal(DateTime startAt, TimeSpan? interval)
        {
            _startAt = startAt;
            _interval = interval;

            var fireSince = startAt - Clock.UtcNow;

            if (fireSince < TimeSpan.Zero)
            {
                fireSince = TimeSpan.Zero;
            }

            var intervalInternal = interval ?? TimeSpan.Zero;

            SetupInternalTimer(fireSince, intervalInternal);
        }

        /// <summary>
        /// Called when internal timer fires, and raises <see cref="AbstractTimer.OnFired"/> event.
        /// </summary>
        protected override void OnInternalTimerFired()
        {
            RaiseOnFired();
        }
    }
}