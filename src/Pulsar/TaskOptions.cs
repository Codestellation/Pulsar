namespace Codestellation.Pulsar
{
    /// <summary>
    /// Contains different options related to task execution
    /// </summary>
    public class TaskOptions
    {
        /// <summary>
        /// Allows or disallow multiple simultaneous task execution
        /// </summary>
        public bool AllowConcurrentExecution { get; set; }
    }
}