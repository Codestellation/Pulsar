using Codestellation.Pulsar.Cron;
using NUnit.Framework;

namespace Codestellation.Pulsar.Tests.Cron
{
    [TestFixture]
    public class CronExpressionBuilderTests
    {
        [Test]
        public void Should_generate_valid_cron_expression()
        {
            var builder = new CronExpressionBuilder
            {
                Seconds = "1-10",
                Minutes = "30",
                Hours = "*",
                DayOfMonth = "1,11,21",
                Month = "3,4-8",
                DayOfWeek = "L",
                Year = "2000-2100"
            };

            var cronExpressionString = builder.ToString();

            Assert.That(cronExpressionString, Is.EqualTo("1-10 30 * 1,11,21 3,4-8 L 2000-2100"));
            Assert.DoesNotThrow(() => builder.ToCronExpression());
        }
    }
}