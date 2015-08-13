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
            Assert.That(_task.Wait(), Is.True, "Task was not called in specifed scheduler");
        }
    }
}