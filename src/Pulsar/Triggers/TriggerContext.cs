using System;

namespace Codestellation.Pulsar.Triggers
{
    public class TriggerContext
    {
        public readonly ITrigger Trigger;

        public TriggerContext(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }
            Trigger = trigger;
        }
    }
}