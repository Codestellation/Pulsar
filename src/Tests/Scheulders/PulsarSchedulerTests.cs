using System;
using System.Threading;
using System.Threading.Tasks;
using Codestellation.Pulsar.Schedulers;
using Codestellation.Pulsar.Triggers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Scheulders
{
    [TestFixture]
    public class PulsarSchedulerTests
    {
        private PulsarScheduler _scheduler;
        private TestTask _task;
        private ManualTrigger _trigger;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new PulsarScheduler();

            _task = new TestTask();
            _trigger = new ManualTrigger();

            _task.AddTrigger(_trigger);

            _scheduler.Add(_task);
        }

        [Test]
        public void Should_call_task_if_scheduler_is_started()
        {
            //given
            _scheduler.Start();
            //when
            _trigger.Fire();
            //then
            Assert.That(_task.WaitForRun(), Is.True, "Task was not called in specifed scheduler");
        }

        [Test]
        public void Should_not_run_task_twice_if_concurrent_execution_is_forbidden()
        {
            _task.Options.AllowConcurrentExecution = false;
            _scheduler.Start();

            var task = Task.Run(() =>
            {
                _trigger.Fire();
                _trigger.Fire();
            });

            task.Wait();
            Thread.Sleep(10);
            _task.Finish();

            if (task.IsFaulted)
            {
                Console.WriteLine(task.Exception);
            }

            Assert.That(_task.WaitForRun(), Is.True);
            Assert.That(_task.CalledTimes, Is.EqualTo(1));
        }
    }
}