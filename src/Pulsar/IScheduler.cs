using System.Collections.Generic;

namespace Codestellation.Pulsar
{
    /// <summary>
    /// Introduces basic interface to operate over scheduled tasks.
    /// </summary>
    public interface IScheduler
    {
        /// <summary>
        /// Enumerates all tasks attacked to scheduler
        /// </summary>
        IEnumerable<ITask> Tasks { get; }

        /// <summary>
        /// Adds a <see cref="ITask"/>to scheduler. Task must have unique Id.
        /// </summary>
        ITask Create(TaskOptions options);

        /// <summary>
        /// Removes specified task from scheduler
        /// </summary>
        void Delete(ITask task);
    }
}