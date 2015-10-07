using System;
using System.Linq;
using Codestellation.Pulsar.FluentApi;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Schedulers;
using Codestellation.Pulsar.Triggers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.FluentApi
{
    [TestFixture]
    public class PulsarSchedulerTests
    {
        private PulsarScheduler _scheduler;

        [SetUp]
        public void SetUp()
        {
            _scheduler = new PulsarScheduler();
            Clock.UtcNowFunction = () => new DateTime(2012, 10, 30, 12, 32, 01, DateTimeKind.Utc);
        }

        [Test]
        public void Should_add_task_with_simple_trigger_with_simple_parameters()
        {
            var interval = TimeSpan.FromMinutes(1);
            _scheduler
                .StartTask(() => Console.WriteLine("Hello World"))
                .RunEvery(interval);

            var task = _scheduler.Tasks.Single();
            var trigger = (SimpleTimerTrigger)task.Triggers.Single();

            Assert.That(trigger.StartAt, Is.EqualTo(new DateTime(2012, 10, 30, 12, 33, 01, DateTimeKind.Utc)));
            Assert.That(trigger.Interval, Is.EqualTo(interval));
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
                .UseParameters(Start.Immediately, Repeat.Minutely);

            var task = _scheduler.Tasks.Single();
            var trigger = task.Triggers.Single();
            Assert.That(trigger, Is.InstanceOf<SimpleTimerTrigger>());
        }

        [TearDown]
        public void RepairClock()
        {
            Clock.UseDefault();
        }
    }
}