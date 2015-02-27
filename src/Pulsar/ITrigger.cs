using System;

namespace Codestellation.Pulsar
{
    public interface ITrigger
    {
        DateTime? LastFiredAt { get; }

        DateTime? NextFireAt { get; }

        void Start();

        void Stop();

        void SetCallback(TriggerCallback callback);
    }
}