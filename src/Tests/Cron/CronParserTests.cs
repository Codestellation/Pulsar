using System.Linq;
using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronParserTests
    {
        [Test]
        public void Generate_correct_range_for_non_zero_starting_ranges()
        {
            var range = CronParser.Range(5, 7).ToArray();
            var expected = new[] {5, 6, 7};
            Assert.That(range, Is.EquivalentTo(expected));
        }
    }
}