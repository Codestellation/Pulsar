using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.Schedulers
{
    public class PulsarScheduler : AbstractScheduler
    {
        private readonly ConcurrentDictionary<Guid, TaskWrap> _tasks;

        /// <summary>
        /// Initialized new instance of <see cref="PulsarScheduler"/>
        /// </summary>
        public PulsarScheduler()
        {
            _tasks = new ConcurrentDictionary<Guid, TaskWrap>();
        }

        /// <summary>
        /// Enumerates all tasks attacked to scheduler
        /// </summary>
        public override IEnumerable<ITask> Tasks
        {
            get
            {
                foreach (var taskWrap in _tasks)
                {
                    yield return taskWrap.Value.Task;
                }
            }
        }

        protected override void AddInternal(ITask task)
        {
            var wrap = new TaskWrap(task, () => Started);
            if (!_tasks.TryAdd(task.Id, wrap))
            {
                throw new InvalidOperationException($"Task {task.Id} with id {task} already added.");
            }

            if (!Started)
            {
                return;
            }
            foreach (var trigger in task.Triggers)
            {
                StartTrigger(wrap, trigger);
            }
        }

        protected override void RemoveInternal(ITask task)
        {
            TaskWrap removed;
            if (!_tasks.TryRemove(task.Id, out removed))
            {
                return;
            }
            foreach (var trigger in removed.Task.Triggers)
            {
                trigger.Stop();
            }
        }

        protected override void StartInternal()
        {
            foreach (var task in _tasks.Values)
            {
                foreach (var trigger in task.Task.Triggers)
                {
                    StartTrigger(task, trigger);
                }
            }
        }

        protected override void StopInternal()
        {
            foreach (var task in _tasks)
            {
                foreach (var trigger in task.Value.Task.Triggers)
                {
                    trigger.Stop();
                }
            }
        }

        private static void StartTrigger(TaskWrap wrap, ITrigger trigger)
        {
            TriggerCallback callback = wrap.OnTriggerCallback;
            trigger.Start(callback);
        }
    }
}