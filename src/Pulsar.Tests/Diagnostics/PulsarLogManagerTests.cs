using Codestellation.Pulsar.Diagnostics;
using NUnit.Framework;
using Shouldly;

namespace Codestellation.Pulsar.Tests.Diagnostics
{
    [TestFixture]
    public class PulsarLogManagerTests
    {
        [Test]
        public void Should_be_able_to_write_to_console_in_full_dotnet()
        {
            PulsarLogManager.CanLogToConsole.ShouldBe(true);
        }
    }
}