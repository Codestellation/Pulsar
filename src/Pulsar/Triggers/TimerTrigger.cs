using System;
using Codestellation.Pulsar.Timers;

namespace Codestellation.Pulsar.Triggers
{
    /// <summary>
    /// Encapsulates basic logic for dependent on timer triggers
    /// </summary>
    public abstract class TimerTrigger : AbstractTrigger
    {
        private readonly ITimer _timer;

        /// <summary>
        /// Inintializes new instance of <see cref="TimerTrigger"/>
        /// </summary>
        /// <param name="timer"></param>
        protected TimerTrigger(ITimer timer)
        {
            if (timer == null)
            {
                throw new ArgumentNullException(nameof(timer));
            }

            _timer = timer;
            _timer.OnFired += OnTimer;
        }

        /// <summary>
        /// Called when timer callback invoked
        /// </summary>
        protected virtual void OnTimer()
        {
            InvokeCallback();
        }

        /// <summary>
        /// Called when <see cref="AbstractTrigger.Stop"/> is called
        /// </summary>
        protected override void OnStop()
        {
            _timer.Stop();
        }

        /// <summary>
        /// Initialize timer with supplied parameters
        /// </summary>
        /// <param name="fireAt">A point in time to fire first time</param>
        /// <param name="interval">An interval to subsequent fires</param>
        protected void SetupTimer(DateTime fireAt, TimeSpan? interval = null)
        {
            _timer.Fire(fireAt, interval);
        }
    }
}