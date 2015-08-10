using System;
using System.Collections.Generic;
using System.Threading;

namespace Codestellation.Pulsar.Tests
{
    public class TestTask : ITask
    {
        private readonly HashSet<ITrigger> _triggers;
        private readonly AutoResetEvent _ran;

        public TestTask()
        {
            Id = Guid.NewGuid();
            Title = $"Test task {Id}";
            _triggers = new HashSet<ITrigger>();

            _ran = new AutoResetEvent(false);
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public IEnumerable<ITrigger> Triggers => _triggers;

        public TestTask AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
            return this;
        }

        public void Run()
        {
            _ran.Set();
        }

        public bool Wait(TimeSpan? timeout = null)
        {
            TimeSpan realTimeOut = timeout ?? TimeSpan.FromSeconds(5);
            return _ran.WaitOne(realTimeOut);
        }
    }
}