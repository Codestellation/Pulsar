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
        /// Adds new task to scheduler. Task must have unique Id. 
        /// </summary>
        IScheduler Add(ITask task);
        /// <summary>
        /// Removes specified task from scheuduler
        /// </summary>
        IScheduler Remove(ITask task);
    }
}