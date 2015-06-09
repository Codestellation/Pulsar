using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public class PulsarScheduler : AbastractScheduler
    {
        private readonly ConcurrentDictionary<Guid, ITask> _tasks;

        public PulsarScheduler()
        {
            _tasks = new ConcurrentDictionary<Guid, ITask>();
        }

        public override IEnumerable<ITask> Tasks
        {
            get { return _tasks.Values; }
        }

        protected override void AddInternal(ITask task)
        {
            if (!_tasks.TryAdd(task.Id, task))
            {
                var message = string.Format("Task {0} with id {1} already added.", task.Id, task);
                throw new InvalidOperationException(message);
            }

            if (!Started)
            {
                return;
            }
            foreach (var trigger in task.Triggers)
            {
                trigger.Start(null);
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
            foreach (var task in _tasks)
            {
                foreach (var trigger in task.Value.Triggers)
                {
                    trigger.Start(null);
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
    }
}