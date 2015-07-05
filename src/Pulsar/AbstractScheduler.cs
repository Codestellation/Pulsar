using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    public abstract class AbstractScheduler : IScheduler, IDisposable, ISchedulerController
    {
        private volatile bool _started;
        private volatile bool _disposed;
        public abstract IEnumerable<ITask> Tasks { get; }

        protected bool Started
        {
            get { return _started; }
        }

        public IScheduler Add(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            EnsureNotDisposed();
            AddInternal(task);
            return this;
        }

        protected abstract void AddInternal(ITask task);

        public IScheduler Remove(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }
            EnsureNotDisposed();
            RemoveInternal(task);
            return this;
        }

        protected abstract void RemoveInternal(ITask task);

        public void Start()
        {
            EnsureNotDisposed();
            _started = true;

            StartInternal();
        }

        protected abstract void StartInternal();

        public void Stop()
        {
            EnsureNotDisposed();
            _started = false;

            StopInternal();
        }

        protected abstract void StopInternal();

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