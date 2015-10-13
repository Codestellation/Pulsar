using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    /// <summary>
    /// Encapsulates any work that should be managed with <see cref="IScheduler"/>
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Returns task options
        /// </summary>
        TaskOptions Options { get; }

        /// <summary>
        /// Collection of <see cref="ITrigger"/> that fire  start task.
        /// </summary>
        IEnumerable<ITrigger> Triggers { get; }

        /// <summary>
        /// Adds specified trigger from task
        /// </summary>
        ITask AddTrigger(ITrigger trigger);

        /// <summary>
        /// Removes specified trigger from task
        /// </summary>
        ITask RemoveTrigger(ITrigger trigger);
    }
}