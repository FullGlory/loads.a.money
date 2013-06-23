using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.Tests.Infrastructure.Messaging
{
    [TestClass]
    public class EnvelopeTests
    {
        public class TestCommand {}

        [TestMethod]
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
