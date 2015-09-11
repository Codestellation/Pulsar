using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.Schedulers
{
    /// <summary>
    /// Encapsulates scheduler task logic
    /// </summary>
    [DebuggerDisplay("{Options}; Triggers = {_triggers.Count}")]
    public class SchedulerTask : ITask
    {
        private readonly Func<bool> _schedulerStarted;
        private int _isStarted;
        private readonly HashSet<ITrigger> _triggers;

        private const int Started = 1;
        private const int NotStarted = 0;

        /// <summary>
        /// Creates new instance of task
        /// </summary>
        public SchedulerTask(TaskOptions options, Func<bool> schedulerStarted)
        {
            Options = options;
            _schedulerStarted = schedulerStarted;
            _triggers = new HashSet<ITrigger>();
        }

        /// <summary>
        /// Gets task execution options
        /// </summary>
        public TaskOptions Options { get; }

        /// <summary>
        /// Collection of <see cref="ITrigger"/> that fire  start task.
        /// </summary>
        public IEnumerable<ITrigger> Triggers => _triggers;

        /// <summary>
        /// Addes trigger to the <see cref="ITask"/>
        /// </summary>
        /// <param name="trigger"></param>
        public ITask AddTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }
            _triggers.Add(trigger);

            StartTrigger(trigger);
            return this;
        }

        /// <summary>
        /// Removes trigger from the <see cref="ITask"/>.
        /// </summary>
        /// <param name="trigger"></param>
        public ITask RemoveTrigger(ITrigger trigger)
        {
            if (trigger == null)
            {
                throw new ArgumentNullException(nameof(trigger));
            }

            if (_triggers.Remove(trigger))
            {
                StopTrigger(trigger);
                (trigger as IDisposable)?.Dispose();
            }
            return this;
        }

        /// <summary>
        /// Starts all triggers attached to task
        /// </summary>
        public void StartTriggers()
        {
            foreach (var trigger in Triggers)
            {
                StartTrigger(trigger);
            }
        }

        /// <summary>
        /// Stops all triggers attached to task
        /// </summary>
        public void StopTriggers()
        {
            foreach (var trigger in Triggers)
            {
                StopTrigger(trigger);
            }
        }

        private void StartTrigger(ITrigger trigger)
        {
            if (_schedulerStarted())
            {
                trigger.Start(OnTriggerCallback);
            }
        }

        private static void StopTrigger(ITrigger trigger)
        {
            trigger.Stop();
        }

        private void OnTriggerCallback(TriggerContext context)
        {
            if (!_schedulerStarted())
            {
                return;
            }

            //if task disallow concurrent execution, try to own lock for call.
            if (!Options.AllowConcurrentExecution && Interlocked.CompareExchange(ref _isStarted, Started, NotStarted) != NotStarted)
            {
                return;
            }
            try
            {
                Options.TaskAction();
            }
            finally
            {
                //Do not need Interlocked, assignment is thread safe
                _isStarted = NotStarted;
            }
        }
    }
}