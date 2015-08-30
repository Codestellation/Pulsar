using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar.Tasks
{
    /// <summary>
    /// Scaffolder to build custom <see cref="ITask"/> implementations
    /// </summary>
    public abstract class AbstractTask : ITask
    {
        private readonly HashSet<ITrigger> _triggers;

        /// <summary>
        /// Unique identifier of task used by <see cref="IScheduler"/> to distinguish task.
        /// </summary>
        public virtual Guid Id { get; protected set; }

        /// <summary>
        /// Gets or sets task execution options
        /// </summary>
        public TaskOptions Options { get; }

        /// <summary>
        /// Collection of <see cref="ITrigger"/> that fire  start task.
        /// </summary>
        public IEnumerable<ITrigger> Triggers => _triggers;

        /// <summary>
        /// Instantiates new instace of <see cref="AbstractTask"/>
        /// </summary>
        protected AbstractTask()
        {
            _triggers = new HashSet<ITrigger>();
            Options = new TaskOptions();
        }

        /// <summary>
        /// Addes trigger to the <see cref="AbstractTask"/>
        /// </summary>
        /// <param name="trigger"></param>
        public void AddTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }
            OnTriggerAdded(trigger);
        }

        /// <summary>
        /// Allows to intercept trigger attach process.
        /// </summary>
        /// <param name="trigger"></param>
        protected virtual void OnTriggerAdded(ITrigger trigger)
        {
            _triggers.Add(trigger);
        }

        /// <summary>
        /// Removes trigger from the <see cref="AbstractTask"/>.
        /// </summary>
        /// <param name="trigger"></param>
        public void RemoveTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }

            OnTriggerRemoved(trigger);
        }

        /// <summary>
        /// Allows to intercept trigger removal process.
        /// </summary>
        /// <param name="trigger"><see cref="ITrigger"/> to remove</param>
        protected virtual void OnTriggerRemoved(ITrigger trigger)
        {
            if (!_triggers.Remove(trigger))
            {
                return;
            }
            trigger.Stop();
            (trigger as IDisposable)?.Dispose();
        }

        /// <summary>
        /// Called by <see cref="IScheduler"/> when one of the task triggers fire.
        /// </summary>
        public abstract void Run();
    }
}