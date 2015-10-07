using System;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    ///  Abstracts work with timer using simple parameters
    /// </summary>
    public class SimpleTimerTrigger : TimerTrigger
    {
        /// <summary>
        /// Gets a point in time to fire trigger first time
        /// </summary>
        public DateTime StartAt { get; }

        /// <summary>
        /// Gets a period of time to fire timer continuously
        /// </summary>
        public TimeSpan? Interval { get; }

        /// <summary>
        /// Initiaitlize new instance of <see cref="SimpleTimerTrigger"/>
        /// </summary>
        /// <param name="startAt">A point in time to fire trigger first time</param>
        /// <param name="interval">A period of time to fire timer continuously</param>
        /// <param name="timer">An instance of <see cref="ITimer"/> </param>
        public SimpleTimerTrigger(DateTime startAt, TimeSpan? interval, ITimer timer)
            : base(timer)
        {
            if (interval <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(interval), "Must be greater than zero");
            }
            StartAt = startAt;
            Interval = interval;
        }

        /// <summary>
        /// Called from <see cref="AbstractTrigger.Start"/> when parameter validation succeed
        /// </summary>
        protected override void OnStart()
        {
            SetupTimer(StartAt, Interval);
        }
    }
}