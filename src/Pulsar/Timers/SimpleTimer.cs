using System;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// Simple wrap over <see cref="System.Threading.Timer"/>
    /// </summary>
    public class SimpleTimer : AbstractTimer
    {
        /// <summary>
        /// Performs initialization of timer
        /// </summary>
        /// <param name="startAt"></param>
        /// <param name="interval"></param>
        protected override void SetupInternal(DateTime startAt, TimeSpan? interval)
        {
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