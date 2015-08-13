using Codestellation.Pulsar.Triggers;

namespace Codestellation.Pulsar
{
    public interface ITrigger
    {
        void Start(TriggerCallback callback);

        void Stop();
    }
}