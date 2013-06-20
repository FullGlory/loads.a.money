using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpreadBet.CommandBus;

namespace SpreadBet.Tests.CommandBus
{
    [TestClass]
    public class SimpleCommandBusTests
    {
        public class TestCommand : ICommand
        {
        }

        [TestMethod]
        public void GivenABusAndACommandHandlerWhenTheCommandIsSentThenTheHandlerIsInvoked()
        {
            // Arrange
            var cmd = new TestCommand();

            var mockCommandHandlerFactory = new Mock<ICommandHandlerFactory>();
            var mockCommandHandler = new Mock<ICommandHandler<TestCommand>>();

            mockCommandHandlerFactory.Setup(x => x.CreateCommandHandler(cmd)).Returns(mockCommandHandler.Object);
            var bus = new SimpleCommandBus(mockCommandHandlerFactory.Object);

            // Act
            (bus as ICommandSender).Send(cmd);

            // Assert
            mockCommandHandlerFactory.VerifyAll();
            mockCommandHandler.Verify(x => x.Execute(cmd));
        }
    }
}
