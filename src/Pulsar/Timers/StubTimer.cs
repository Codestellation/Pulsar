using System;

namespace Codestellation.Pulsar.Timers
{
    /// <summary>
    /// This class is intended to be used in tests. 
    /// </summary>
    public class StubTimer : ITimer
    {
        public event Action OnFired = delegate {};
        
        public void Fire(DateTime startAt, TimeSpan? interval = null)
        {
            StartAt = startAt;
            Interval = interval;
            Started = true;
        }

        public TimeSpan? Interval { get; private set; }

        public DateTime StartAt { get; private set; }

        public bool Started { get; private set; }

        public bool Stopped { get; private set; }

        public void Stop()
        {
            Stopped = true;
        }

        public void Fire()
        {
            OnFired();
        }
    }
}