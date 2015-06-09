using System;

namespace Codestellation.Pulsar
{
    public class TriggerContext
    {
        public readonly ITrigger Trigger;

        public TriggerContext(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException("trigger");
            }
            Trigger = trigger;
        }
    }
}