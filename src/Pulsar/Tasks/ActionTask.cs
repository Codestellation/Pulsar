using System;

namespace Codestellation.Pulsar.Tasks
{
    /// <summary>
    /// Simple task that utilize <see cref="Action"/>
    /// </summary>
    public class ActionTask : AbstractTask
    {
        private readonly Action _action;

        /// <summary>
        /// Initialize new instance of <see cref="ActionTask"/> class
        /// </summary>
        /// <param name="name"></param>
        /// <param name="action"></param>
        public ActionTask(Action action, string name = null)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            _action = action;
            Id = Guid.NewGuid();
            Title = string.IsNullOrWhiteSpace(name) ? $"Task {Id}" : name;
        }

        /// <summary>
        /// Called by <see cref="IScheduler"/> when one of the task triggers fire.
        /// </summary>
        public override void Run()
        {
            _action();
        }
    }
}