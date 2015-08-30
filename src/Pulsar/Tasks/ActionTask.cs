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
        /// <param name="action"></param>
        public ActionTask(Action action)
        {
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            _action = action;
            Id = Guid.NewGuid();
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