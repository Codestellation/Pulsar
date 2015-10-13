using System;

namespace Codestellation.Pulsar
{
    /// <summary>
    /// Contains different options related to task execution
    /// </summary>
    public class TaskOptions
    {
        /// <summary>
        /// Initializes new instance of <see cref="TaskOptions"/>
        /// </summary>
        public TaskOptions()
            : this(Guid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes new instance of <see cref="TaskOptions"/>
        /// </summary>
        public TaskOptions(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentException("Must be not empty", nameof(id));
            }
            Id = id;
        }

        /// <summary>
        /// Unique identifier of task used by <see cref="IScheduler"/> to distinguish task.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Human readable task name.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Allows or disallow multiple simultaneous task execution
        /// </summary>
        public bool AllowConcurrentExecution { get; set; }

        /// <summary>
        /// Called by <see cref="IScheduler"/> when one of the task triggers fire.
        /// </summary>
        public Action TaskAction { get; set; }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        public override string ToString()
        {
            return $"{Id.ToString()} - '{Title}'";
        }
    }
}