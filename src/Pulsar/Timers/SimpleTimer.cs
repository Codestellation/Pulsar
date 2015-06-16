using System;
using Codestellation.Pulsar.Misc;

namespace Codestellation.Pulsar.Timers
{
    public class SimpleTimer : AbstractTimer
    {
        protected override void SetupInternal(DateTime startAt, TimeSpan? interval)
        {
            var fireSince = startAt - Clock.UtcNow;

            var intervalInternal = interval ?? TimeSpan.Zero;

            SetupInternalTimer(fireSince, intervalInternal);
        }

        protected override void OnInternalTimerFired()
        {
            Callback();
        }
    }
}