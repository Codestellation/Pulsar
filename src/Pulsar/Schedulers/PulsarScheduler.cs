using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.Schedulers
{
    public class PulsarScheduler : IScheduler, IDisposable, ISchedulerController
    {
        private readonly ConcurrentDictionary<Guid, TaskWrap> _tasks;
        private volatile bool _started;
        private volatile bool _disposed;

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
        public IEnumerable<ITask> Tasks
        {
            get
            {
                foreach (var taskWrap in _tasks)
                {
                    yield return taskWrap.Value.Task;
                }
            }
        }

        private static void StartTrigger(TaskWrap wrap, ITrigger trigger)
        {
            TriggerCallback callback = wrap.OnTriggerCallback;
            trigger.Start(callback);
        }

        public IScheduler Add(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            EnsureNotDisposed();
            var wrap = new TaskWrap(task, () => _started);
            if (!_tasks.TryAdd(task.Id, wrap))
            {
                throw new InvalidOperationException($"Task {task.Id} with id {task} already added.");
            }

            if (!_started)
            {
                return this;
            }

            foreach (var trigger in task.Triggers)
            {
                StartTrigger(wrap, trigger);
            }
            return this;
        }

        public IScheduler Remove(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            EnsureNotDisposed();
            TaskWrap removed;
            if (!_tasks.TryRemove(task.Id, out removed))
            {
                return this;
            }
            foreach (var trigger in removed.Task.Triggers)
            {
                trigger.Stop();
            }
            return this;
        }

        public void Start()
        {
            EnsureNotDisposed();
            _started = true;

            foreach (var task in _tasks.Values)
            {
                foreach (var trigger in task.Task.Triggers)
                {
                    StartTrigger(task, trigger);
                }
            }
        }

        public void Stop()
        {
            EnsureNotDisposed();
            _started = false;

            foreach (var task in _tasks)
            {
                foreach (var trigger in task.Value.Task.Triggers)
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
                throw new InvalidOperationException("Scheduler was disposed");
            }
        }
    }
}