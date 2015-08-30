using System;
using System.Collections.Generic;
using System.Threading;

namespace Codestellation.Pulsar.Tests
{
    public class TestTask : ITask
    {
        private readonly HashSet<ITrigger> _triggers;
        private readonly AutoResetEvent _ran;
        private readonly AutoResetEvent _finished;
        public int CalledTimes;

        public TestTask()
        {
            Id = Guid.NewGuid();
            Title = $"Test task {Id}";
            _triggers = new HashSet<ITrigger>();
            Options = new TaskOptions();

            _ran = new AutoResetEvent(false);
            _finished = new AutoResetEvent(false);
        }

        public Guid Id { get; set; }

        public string Title { get; set; }

        public TaskOptions Options { get; }

        public IEnumerable<ITrigger> Triggers => _triggers;

        public TestTask AddTrigger(ITrigger trigger)
        {
            _triggers.Add(trigger);
            return this;
        }

        public void Run()
        {
            Interlocked.Increment(ref CalledTimes);
            _ran.Set();
            _finished.WaitOne();
        }

        public bool WaitForRun(TimeSpan? timeout = null)
        {
            TimeSpan realTimeOut = timeout ?? TimeSpan.FromSeconds(5);
            return _ran.WaitOne(realTimeOut);
        }

        public void Finish()
        {
            _finished.Set();
        }
    }
}