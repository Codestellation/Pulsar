using System;
using System.Diagnostics;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Fires on schedule specified in cron expression
    /// </summary>
    [DebuggerDisplay("Next = {NextFireAt}")]
    public class ScheduleTrigger : TimerTrigger
    {
        private readonly ISchedule _calculator;

        /// <summary>
        /// Returns nearest timer to fire
        /// </summary>
        public DateTime? NextFireAt
        {
            get
            {
                var nextAt = _calculator.NextAt;
                if (nextAt.HasValue && nextAt.Value.Kind != DateTimeKind.Utc)
                {
                    throw new InvalidOperationException($"Occurrence calculator must return UTC datetime, but was {nextAt.Value.Kind}");
                }
                return nextAt;
            }
        }

        /// <summary>
        /// Initialize new instance of <see cref="ScheduleTrigger"/>
        /// </summary>
        /// <param name="calculator"></param>
        /// <param name="timer">Timer implementation
        /// <remarks>It's recommended to use <see cref="PreciseTimer"/> for long intervals to avoid timer drift issues</remarks>
        /// </param>
        /// <exception cref="ArgumentNullException"></exception>
        public ScheduleTrigger(ISchedule calculator, ITimer timer)
            : base(timer)
        {
            if (calculator == null)
            {
                throw new ArgumentNullException(nameof(calculator));
            }
            _calculator = calculator;
        }

        /// <summary>
        /// Invokes callback and restarts internal timer
        /// </summary>
        protected override void OnTimer()
        {
            OnStart();
            InvokeCallback();
        }

        /// <summary>
        /// Starts timer if it's possible due to cron expression
        /// </summary>
        protected override void OnStart()
        {
            if (NextFireAt.HasValue)
            {
                SetupTimer(NextFireAt.Value);
            }
        }
    }
}