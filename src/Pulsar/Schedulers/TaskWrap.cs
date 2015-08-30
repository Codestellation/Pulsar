using System;
using System.Threading;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.Schedulers
{
    internal class TaskWrap
    {
        public readonly ITask Task;
        private readonly Func<bool> _schedulerStarted;
        private int _isStarted;

        private const int Started = 1;
        private const int NotStarted = 0;

        public TaskWrap(ITask task, Func<bool> schedulerStarted)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }
            Task = task;
            _schedulerStarted = schedulerStarted;
        }

        public void OnTriggerCallback(TriggerContext context)
        {
            if (!_schedulerStarted())
            {
                return;
            }

            //if task disallow concurrent execution, try to own lock for call.
            if (!Task.Options.AllowConcurrentExecution && Interlocked.CompareExchange(ref _isStarted, Started, NotStarted) != NotStarted)
            {
                return;
            }
            try
            {
                Task.Run();
            }
            finally
            {
                //Do not need Interlocked, assignment is thread safe
                _isStarted = NotStarted;
            }
        }
    }
}