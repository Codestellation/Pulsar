using System;
using Codestellation.Pulsar.Cron;
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
        /// Creates new intannse of <see cref="ITask"/> and adds it to <see cref="IScheduler"/> with specified <see cref="Action"/>
        /// </summary>
        public static ITask StartTask(this IScheduler self, Action taskAction)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            if (taskAction == null)
            {
                throw new ArgumentNullException(nameof(taskAction));
            }

            var options = new TaskOptions
            {
                TaskAction = taskAction
            };
            var task = self.Create(options);
            return task;
        }

        /// <summary>
        /// Adds cron trigger to <see cref="ITask"/>
        /// </summary>
        public static ITask UseCron(this ITask self, string cronExpression, ITimer timer = null, TimeZoneInfo timeZone = null)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            timer = timer ?? new PreciseTimer();
            timeZone = timeZone ?? TimeZoneInfo.Local;
            var schedule = new CronSchedule(new CronExpression(cronExpression), timeZone);
            var trigger = new ScheduleTrigger(schedule, timer);

            self.AddTrigger(trigger);

            return self;
        }

        /// <summary>
        /// Adds simple trigger to <see cref="ITask"/>
        /// </summary>
        public static ITask UseParameters(this ITask self, DateTime startAt, TimeSpan? interval, ITimer timer = null)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            timer = timer ?? new SimpleTimer();
            var trigger = new SimpleTimerTrigger(startAt, interval, timer);

            self.AddTrigger(trigger);
            return self;
        }

        /// <summary>
        /// Adds simple trigger to <see cref="ITask"/>
        /// </summary>
        public static ITask RunEvery(this ITask self, TimeSpan interval, ITimer timer = null)
        {
            if (self == null)
            {
                throw new ArgumentNullException(nameof(self));
            }

            timer = timer ?? new SimpleTimer();
            var startAt = Start.After(interval);
            var trigger = new SimpleTimerTrigger(startAt, interval, timer);

            self.AddTrigger(trigger);
            return self;
        }
    }
}