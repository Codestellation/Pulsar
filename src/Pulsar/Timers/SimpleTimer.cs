using System;

namespace Codestellation.Pulsar.Timers
{
    public class SimpleTimer : AbstractTimer
    {

        protected override void SetupInternal(DateTime fireAt, TimeSpan? interval)
        {
            var fireSince = fireAt - UtcNow();

            var intervalInternal = interval ?? TimeSpan.Zero;

            SetupInternalTimer(fireSince, intervalInternal);
        }

        protected override void OnInternalTimerFired()
        {
            Callback();
        }
    }
}