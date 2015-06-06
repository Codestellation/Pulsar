namespace Codestellation.Pulsar
{
    /// <summary>
    /// Controls <see cref="IScheduler"/> behaviour. 
    /// </summary>
    public interface ISchedulerController
    {
        void Start();

        void Stop();
    }
}