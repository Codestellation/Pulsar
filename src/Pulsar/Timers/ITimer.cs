using System;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// Timers used by few <see cref="ITrigger"/> internally. Not intended (but not prohibited) from direct 
    /// </summary>
    public interface ITimer
    {
        /// <summary>
        /// Fires on time is come.
        /// </summary>
        event Action OnFired;

        /// <summary>
        /// Forces <see cref="ITimer"/> to fire events with specified parameters
        /// </summary>
        /// <param name="startAt">First datetime when timers must fire. Must be UTC or local.</param>
        /// <remarks>
        /// if  is below or equal to  <see cref="Pulsar.Clock"/>
        /// </remarks>
        /// <param name="interval">Period of timer </param>
        void Fire(DateTime startAt, TimeSpan? interval = null);

        /// <summary>
        /// Forces timer to stop generating events. 
        /// </summary>
        void Stop();
    }
}