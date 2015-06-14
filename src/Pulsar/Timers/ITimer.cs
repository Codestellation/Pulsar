using System;

namespace Codestellation.Pulsar.Timers
{
    public interface ITimer
    {
        event Action OnFired;

        void Fire(DateTime startAt, TimeSpan? interval);
    }
}