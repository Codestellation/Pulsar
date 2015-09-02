using System;
using System.Linq;
using Codestellation.Pulsar.FluentApi;
using Codestellation.Pulsar.Schedulers;
using Codestellation.Pulsar.Triggers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Extensions
{
    [TestFixture]
    public class PulsarSchedulerTests
    {
        private PulsarScheduler _scheduler;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new PulsarScheduler();
        }

        [Test]
        public void Should_add_task_with_cron_trigger_to_scheduler()
        {
            _scheduler
                .StartTask(() => Console.WriteLine("Hello World"))
                .UseCron(" * 0 10 * * ? ");

            var task = _scheduler.Tasks.Single();
            var trigger = task.Triggers.Single();
            Assert.That(trigger, Is.InstanceOf<CronTrigger>());
        }

        [Test]
        public void Should_add_task_with_simple_trigger_to_scheduler()
        {
            _scheduler
                .StartTask(() => Console.WriteLine("Hello World"))
                .UseParameters(Start.Immediately, Repeat.Every.Minute);

            var task = _scheduler.Tasks.Single();
            var trigger = task.Triggers.Single();
            Assert.That(trigger, Is.InstanceOf<SimpleTimerTrigger>());
        }
    }
}