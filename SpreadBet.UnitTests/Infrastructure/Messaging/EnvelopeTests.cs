using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.Tests.Infrastructure.Messaging
{
    [TestFixture]
    public class EnvelopeTests
    {
        public class TestCommand {}

        [Test]
        public void ImplicitConversion_WithCommand_ReturnsEnvelope()
        {
            // Arrange
            var cmd = new TestCommand{};

            // Act
            var e = (Envelope<TestCommand>)cmd;

            // Assert
            Assert.IsNotNull(e);
            Assert.AreEqual(cmd, e.Body);
        }
    }
}
