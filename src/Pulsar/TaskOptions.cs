namespace Codestellation.Pulsar
{
    /// <summary>
    /// Contains different options related to task execution
    /// </summary>
    public class TaskOptions
    {
        /// <summary>
        /// Human readable task name.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Allows or disallow multiple simultaneous task execution
        /// </summary>
        public bool AllowConcurrentExecution { get; set; }
    }
}