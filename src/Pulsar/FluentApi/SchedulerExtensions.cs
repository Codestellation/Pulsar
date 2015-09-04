using System;
using Codestellation.Pulsar.Tasks;
using Codestellation.Pulsar.Timers;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.FluentApi
{
    /// <summary>
    ///  Creates fluent API to quickly manage tasks.
    /// </summary>
    public static class SchedulerExtensions
    {
        /// <summary>
        /// Creates new <see cref="ActionTask"/> and adds it to <see cref="IScheduler"/>
        /// </summary>
        public static ActionTask StartTask(this IScheduler self, Action taskAction)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (taskAction == null)
            {
                throw new ArgumentNullException(nameof(taskAction));
            }

            var task = new ActionTask(taskAction);
            self.Add(task);
            return task;
        }

        /// <summary>
        /// Adds cron trigger to <see cref="AbstractTask"/>
        /// </summary>
        public static void UseCron(this AbstractTask self, string cronExpression, ITimer timer = null, TimeZoneInfo timeZone = null)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            timer = timer ?? new PreciseTimer();
            timeZone = timeZone ?? TimeZoneInfo.Local;
            var trigger = new CronTrigger(cronExpression, timeZone, timer);

            self.AddTrigger(trigger);
        }

        /// <summary>
        /// Adds simple trigger to <see cref="AbstractTask"/>
        /// </summary>
        public static void UseParameters(this AbstractTask self, DateTime startAt, TimeSpan? interval, ITimer timer = null)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            timer = timer ?? new SimpleTimer();
            var trigger = new SimpleTimerTrigger(startAt, interval, timer);

            self.AddTrigger(trigger);
        }
    }
}