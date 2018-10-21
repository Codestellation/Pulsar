using System;
using System.Threading;
using System.Threading.Tasks;
using Codestellation.Pulsar.Schedulers;
using Codestellation.Pulsar.Triggers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Schedulers
{
    [TestFixture]
    public class PulsarSchedulerTests
    {
        private PulsarScheduler _scheduler;
        private TestAction _action;
        private ManualTrigger _trigger;
        private ITask _task;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new PulsarScheduler();

            _action = new TestAction();
            _trigger = new ManualTrigger();

            _task = _scheduler.Create(_action.Options);

            _task.AddTrigger(_trigger);
        }

        [Test]
        public void Should_not_fail_on_dispose()
        {
            _scheduler.Dispose();
        }

        [Test]
        public void Should_call_task_if_scheduler_is_started()
        {
            //given
            _scheduler.Start();
            //when
            _trigger.Fire();
            //then
            Assert.That(_action.WaitForRun(), Is.True, "Task was not called in specifed scheduler");
        }

        [Test]
        public void Should_not_run_task_twice_if_concurrent_execution_is_forbidden()
        {
            _action.Options.AllowConcurrentExecution = false;
            _scheduler.Start();

            var task = Task.Run(() =>
            {
                _trigger.Fire();
                _trigger.Fire();
            });

            task.Wait();
            Thread.Sleep(2000);
            _action.Finish();

            if (task.IsFaulted)
            {
                Console.WriteLine(task.Exception);
            }

            Assert.That(_action.WaitForRun(), Is.True);
            Assert.That(_action.CalledTimes, Is.EqualTo(1));
        }
    }
}