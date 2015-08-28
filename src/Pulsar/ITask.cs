using System;
using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    /// <summary>
    /// Encapsulates any work that should be managed with <see cref="IScheduler"/>
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Unique identifier of task used by <see cref="IScheduler"/> to distinguish task.
        /// </summary>
        Guid Id { get; }

        /// <summary>
        /// Human readable task name.
        /// </summary>
        string Title { get; }

        /// <summary>
        /// Collection of <see cref="ITrigger"/> that fire  start task.
        /// </summary>
        IEnumerable<ITrigger> Triggers { get; }

        /// <summary>
        /// Called by <see cref="IScheduler"/> when one of the task triggers fire.
        /// </summary>
        void Run();
    }
}