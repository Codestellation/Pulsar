using System;
using System.Threading;

namespace Codestellation.Pulsar.Tests
{
    public class TestAction
    {
        private readonly AutoResetEvent _ran;
        private readonly AutoResetEvent _finished;
        public int CalledTimes;

        public TestAction()
        {
            Options = new TaskOptions
            {
                TaskAction = Run
            };
            _ran = new AutoResetEvent(false);
            _finished = new AutoResetEvent(false);
        }

        public TaskOptions Options { get; }

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