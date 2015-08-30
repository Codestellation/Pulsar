using System;
using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar.Schedulers
{
    internal class TaskWrap
    {
        public readonly ITask Task;
        private readonly Func<bool> _schedulerStarted;

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
            Task.Run();
        }
    }
}