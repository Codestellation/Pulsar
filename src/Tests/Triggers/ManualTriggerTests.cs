using Codestellation.Pulsar.Triggers;
using NUnit.Framework;
using Shouldly;

namespace Codestellation.Pulsar.Tests.Triggers
{
    [TestFixture]
    public class ManualTriggerTests
    {
        private TestTask _task;
        private ManualTrigger _trigger;

        [SetUp]
        public void SetUp()
        {
            _task = new TestTask();
            _trigger = new ManualTrigger();

            _task.AddTrigger(_trigger);
        }

        [Test]
        public void Should_not_fire_if_trigger_is_not_started()
        {
            //given
            _trigger.Stop();
            //when
            _trigger.Fire();
            //then
            _task.WaitForRun().ShouldBe(false, "Task was called when trigger is not started");
        }

        [Test]
        public void Should_fire_if_trigger_is_started()
        {
            //given
            _trigger.Start(context => _task.Run());
            //when
            _trigger.Fire();
            //then
            _task.WaitForRun().ShouldBe(true, "Task was called when trigger is not started");
        }
    }
}