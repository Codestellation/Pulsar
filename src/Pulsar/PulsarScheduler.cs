using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public class PulsarScheduler : AbstractScheduler
    {
        private readonly ConcurrentDictionary<Guid, ITask> _tasks;

        public PulsarScheduler()
        {
            _tasks = new ConcurrentDictionary<Guid, ITask>();
        }

        public override IEnumerable<ITask> Tasks => _tasks.Values;

        protected override void AddInternal(ITask task)
        {
            if (!_tasks.TryAdd(task.Id, task))
            {
                throw new InvalidOperationException($"Task {task.Id} with id {task} already added.");
            }

            if (!Started)
            {
                return;
            }
            foreach (var trigger in task.Triggers)
            {
                StartTrigger(task, trigger);
            }
        }

        protected override void RemoveInternal(ITask task)
        {
            ITask removed;
            if (!_tasks.TryRemove(task.Id, out removed))
            {
                return;
            }
            foreach (var trigger in removed.Triggers)
            {
                trigger.Stop();
            }
        }

        protected override void StartInternal()
        {
            foreach (var task in _tasks.Values)
            {
                foreach (var trigger in task.Triggers)
                {
                    StartTrigger(task, trigger);
                }
            }
        }

        protected override void StopInternal()
        {
            foreach (var task in _tasks)
            {
                foreach (var trigger in task.Value.Triggers)
                {
                    trigger.Stop();
                }
            }
        }

        private static void StartTrigger(ITask task, ITrigger trigger)
        {
            TriggerCallback callback = context => task.Run();
            trigger.Start(callback);
        }
    }
}