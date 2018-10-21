using Codestellation.Pulsar.Triggers;
using NUnit.Framework;
using Shouldly;

namespace Codestellation.Pulsar.Tests.Triggers
{
    [TestFixture]
    public class ManualTriggerTests
    {
        private TestAction _action;
        private ManualTrigger _trigger;

        [SetUp]
        public void SetUp()
        {
            _action = new TestAction();
            _trigger = new ManualTrigger();
        }

        [Test]
        public void Should_not_fire_if_trigger_is_not_started()
        {
            //given
            _trigger.Stop();
            //when
            _trigger.Fire();
            //then
            _action.WaitForRun().ShouldBe(false, "Task was called when trigger is not started");
        }

        [Test]
        public void Should_fire_if_trigger_is_started()
        {
            //given
            _trigger.Start(context => _action.Run());
            //when
            _trigger.Fire();
            //then
            _action.WaitForRun().ShouldBe(true, "Task was called when trigger is not started");
        }
    }
}