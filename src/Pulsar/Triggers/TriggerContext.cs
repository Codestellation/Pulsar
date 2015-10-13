using System;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Information about trigger that invokes callbacke
    /// </summary>
    public class TriggerContext
    {
        /// <summary>
        /// Trigger that invokes callback
        /// </summary>
        public readonly ITrigger Trigger;

        /// <summary>
        /// Initialize new instance of <see cref="TriggerContext"/>
        /// </summary>
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