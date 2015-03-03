using System;
using System.Collections.Concurrent;

namespace Codestellation.Pulsar
{
    public class PulsarScheduler : IScheduler, IDisposable
    {
        private readonly ConcurrentDictionary<Guid, ITask> _tasks;
        private volatile bool _started;

        public PulsarScheduler()
        {
            _tasks = new ConcurrentDictionary<Guid, ITask>();
        }

        public void Schedule(ITask task)
        {
            if (!_tasks.TryAdd(task.Id, task))
            {
                var message = string.Format("Task {0} with id {1} already scheduled.", task.Id, task);
                throw new InvalidOperationException(message);
            }

            if (_started)
            {
                foreach (var trigger in task.Triggers)
                {
                    trigger.Start();
                }
            }

        }

        public void Start()
        {
            _started = true;

            foreach (var task in _tasks)
            {
                foreach (var trigger in task.Value.Triggers)
                {
                    trigger.Start();
                }
            }
        }

        public void Stop()
        {
            _started = false;

            foreach (var task in _tasks)
            {
                foreach (var trigger in task.Value.Triggers)
                {
                    trigger.Stop();
                }
            }
        }


        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}