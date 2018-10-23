using System;
using System.Threading;
using Codestellation.Pulsar.Misc;
using Codestellation.Pulsar.Timers;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Timers
{
    [TestFixture(typeof(SimpleTimer))]
    [TestFixture(typeof(PreciseTimer))]
    public class TimerTests<TTimer>
        where TTimer : ITimer, IDisposable, new()
    {
        private TTimer _timer;

        [SetUp]
        public void Setup()
        {
            _timer = new TTimer();
        }

        [Test]
        public void Should_not_fail_if_date_time_is_utc()
        {
            //given
            var called = new ManualResetEventSlim(false);
            var utcNow = DateTime.UtcNow;
            Clock.UtcNowFunction = () => utcNow;

            _timer.OnFired += () => called.Set();
            var pastTime = utcNow.AddDays(-1);

            //when
            _timer.Fire(pastTime, TimeSpan.FromSeconds(10));

            //then
            var isFired = called.Wait(TimeSpan.FromSeconds(1));
            Assert.That(isFired, Is.True);
        }

        [Test]
        public void Should_fire_once_if_interval_not_specified()
        {
            //given
            var counter = new CountdownEvent(10);
            _timer.OnFired += () => counter.Signal();

            //when
            _timer.Fire(DateTime.UtcNow);

            //then
            counter.Wait(TimeSpan.FromSeconds(1));
            counter.Wait(TimeSpan.FromSeconds(1)); // To ensure nothing's gonna happen after the first time.  
            
            Assert.That(counter.CurrentCount, Is.EqualTo(9));
        }

        [Test]
        public void Should_fire_multiple_times_if_interval_set()
        {
            //given
            var counter = 0;
            _timer.OnFired += () => Interlocked.Increment(ref counter);

            //when
            _timer.Fire(DateTime.UtcNow, TimeSpan.FromMilliseconds(100));

            //then
            Thread.Sleep(1000);
            Assert.That(counter, Is.GreaterThan(5));
        }

        [Test]
        public void Should_not_fail_on_double_dispose()
        {
            _timer.Dispose();
            Assert.DoesNotThrow(() => _timer.Dispose());
        }

        [Test]
        public void Should_not_fail_on_stop_if_dispose()
        {
            _timer.Dispose();
            Assert.DoesNotThrow(() => _timer.Stop());
        }

        [Test]
        public void Should_throw_if_disposed()
        {
            _timer.Dispose();
            Assert.Throws<ObjectDisposedException>(() => _timer.Fire(DateTime.UtcNow));
        }

        [TearDown]
        public void RepairClock()
        {
            _timer.Dispose();
            Clock.UseDefault();
        }
    }
}