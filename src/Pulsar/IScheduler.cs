namespace Codestellation.Pulsar
{
    public interface IScheduler
    {
        void Schedule(ITask task);
    }
}