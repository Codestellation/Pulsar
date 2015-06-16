using System;

namespace Codestellation.Pulsar
{
    public interface ITrigger
    {
        DateTime? NextFireAt { get; }

        void Start(TriggerCallback callback);

        void Stop();
    }
}