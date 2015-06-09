using System;

namespace Codestellation.Pulsar
{
    public interface ITrigger
    {
        DateTime? LastFiredAt { get; }

        DateTime? NextFireAt { get; }

        void Start(TriggerCallback callback);

        void Stop();
    }
}