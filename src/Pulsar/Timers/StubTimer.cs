using System;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// This class is intended to be used in tests.
    /// </summary>
    public class StubTimer : ITimer
    {
        /// <summary>
        /// Raised by <see cref="RaiseOnFired"/> method
        /// </summary>
        public event Action OnFired = delegate { };

        /// <summary>
        /// Saves parameters and sets <see cref="Started"/> to true
        /// </summary>
        /// <param name="startAt"></param>
        /// <param name="interval"></param>
        public void Fire(DateTime startAt, TimeSpan? interval = null)
        {
            StartAt = startAt;
            Interval = interval;
            Started = true;
        }

        /// <summary>
        /// Interval supplied in <see cref="Fire"/> method
        /// </summary>
        public TimeSpan? Interval { get; private set; }

        /// <summary>
        /// Datetime supplied in <see cref="Fire"/> method
        /// </summary>
        public DateTime? StartAt { get; private set; }

        /// <summary>
        /// True if <see cref="Fire"/> was called at least once, false otherwise
        /// </summary>
        public bool Started { get; private set; }

        /// <summary>
        /// True if <see cref="Stop"/> was called at lease once, false otherwise
        /// </summary>
        public bool Stopped { get; private set; }

        /// <summary>
        /// Sets <see cref="Stopped"/> property to true
        /// </summary>
        public void Stop()
        {
            Stopped = true;
        }

        /// <summary>
        /// Raises <see cref="OnFired"/> event
        /// </summary>
        public void RaiseOnFired()
        {
            OnFired();
        }
    }
}