namespace Codestellation.Pulsar
{
    /// <summary>
    /// Controls <see cref="IScheduler"/> behaviour.
    /// <remarks>
    /// If schduler is not started, triggers callback will be ignored even in case on manual trigger fire.
    /// </remarks>
    /// </summary>
    public interface ISchedulerController
    {
        /// <summary>
        /// Forces a <see cref="IScheduler"/> to start triggers from attached instances <see cref="ITask"/> and
        /// </summary>
        void Start();

        /// <summary>
        /// Stops all triggers and force <see cref="IScheduler"/> to prevent Task.Run calls
        /// </summary>
        void Stop();
    }
}