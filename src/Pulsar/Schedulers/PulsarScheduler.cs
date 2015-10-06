using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using Codestellation.Pulsar.Diagnostics;

namespace Codestellation.Pulsar.Schedulers
{
    /// <summary>
    /// Manages tasks lifecycle
    /// </summary>
    [DebuggerDisplay("Count = {_tasks.Count}")]
    public class PulsarScheduler : IScheduler, IDisposable, ISchedulerController
    {
        private static readonly string LoggerName = typeof(PulsarScheduler).FullName;

        private readonly ConcurrentDictionary<Guid, SchedulerTask> _tasks;
        private volatile bool _started;
        private volatile bool _disposed;

        /// <summary>
        /// Initialized new instance of <see cref="PulsarScheduler"/>
        /// </summary>
        public PulsarScheduler()
        {
            _tasks = new ConcurrentDictionary<Guid, SchedulerTask>();
        }

        /// <summary>
        /// Enumerates all tasks attacked to scheduler
        /// </summary>
        public IEnumerable<ITask> Tasks
        {
            get
            {
                foreach (var task in _tasks)
                {
                    yield return task.Value;
                }
            }
        }

        /// <summary>
        /// Initializes new task instance and adds it to scheduler collection
        /// </summary>
        /// <param name="options"></param>
        /// <returns>Returns created task</returns>
        public ITask Create(TaskOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }
            EnsureNotDisposed();

            var task = new SchedulerTask(options, () => _started);

            if (!_tasks.TryAdd(task.Options.Id, task))
            {
                throw new InvalidOperationException($"Task {task.Options.Id} already added");
            }

            if (PulsarLogger.IsDebugEnabled(LoggerName))
            {
                PulsarLogger.Debug(LoggerName, $"Created task {task.Options.Id} '{task.Options.Title}'");
            }

            return task;
        }

        /// <summary>
        /// Removes task from scheduler
        /// </summary>
        public void Delete(ITask task)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            EnsureNotDisposed();
            SchedulerTask removed;
            if (!_tasks.TryRemove(task.Options.Id, out removed))
            {
                removed.StopTriggers();
            }

            if (PulsarLogger.IsDebugEnabled(LoggerName))
            {
                PulsarLogger.Debug(LoggerName, $"Removed task {task.Options.Id} '{task.Options.Title}'");
            }
        }

        /// <summary>
        /// Forces a <see cref="IScheduler"/> to start triggers from attached instances <see cref="ITask"/>
        /// </summary>
        public void Start()
        {
            EnsureNotDisposed();
            _started = true;

            PulsarLogger.Debug(nameof(PulsarScheduler), "Starting");

            foreach (var task in _tasks.Values)
            {
                task.StartTriggers();
            }

            PulsarLogger.Debug(nameof(PulsarScheduler), "Started");
        }

        /// <summary>
        /// Stops all triggers and force <see cref="IScheduler"/> to prevent <see cref="TaskOptions.TaskAction"/> invocation
        /// </summary>
        public void Stop()
        {
            if (_disposed)
            {
                return;
            }

            PulsarLogger.Debug(nameof(PulsarScheduler), "Stopping");

            StopInternal();

            PulsarLogger.Debug(nameof(PulsarScheduler), "Stopped");
        }

        private void StopInternal()
        {
            _started = false;

            foreach (var task in _tasks)
            {
                task.Value.StopTriggers();
            }
        }

        /// <summary>
        /// Stops all task and triggres
        /// </summary>
        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            PulsarLogger.Debug(nameof(PulsarScheduler), "Disposing");

            _disposed = true;
            StopInternal();

            PulsarLogger.Debug(nameof(PulsarScheduler), "Disposed");
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