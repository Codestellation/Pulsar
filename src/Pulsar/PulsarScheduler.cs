using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public class PulsarScheduler : IScheduler, IDisposable, ISchedulerController
    {
        private readonly ConcurrentDictionary<Guid, ITask> _tasks;
        private volatile bool _started;
        private volatile bool _disposed;

        public PulsarScheduler()
        {
            _tasks = new ConcurrentDictionary<Guid, ITask>();
        }

        public IEnumerable<ITask> Tasks
        {
            get { return _tasks.Values; }
        }

        public IScheduler Add(ITask task)
        {
            EnsureNotDisposed();
            if (!_tasks.TryAdd(task.Id, task))
            {
                var message = string.Format("Task {0} with id {1} already added.", task.Id, task);
                throw new InvalidOperationException(message);
            }

            if (_started)
            {
                foreach (var trigger in task.Triggers)
                {
                    trigger.Start();
                }
            }
            return this;
        }

        public IScheduler Remove(ITask task)
        {
            ITask removed;
            if (_tasks.TryRemove(task.Id, out removed))
            {
                foreach (var trigger in removed.Triggers)
                {
                    trigger.Stop();
                }
            }
            return this;
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
            if (_disposed)
            {
                return;
            }
            _disposed = true;
            Stop();
        }

        private void EnsureNotDisposed()
        {
            if (_disposed)
            {
                throw new InvalidOperationException();
            }
        }
    }
}