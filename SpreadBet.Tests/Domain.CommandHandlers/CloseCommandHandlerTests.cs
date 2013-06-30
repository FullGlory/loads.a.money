namespace SpreadBet.Tests.Domain.CommandHandlers
{
    using System;
    using System.Linq.Expressions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ploeh.AutoFixture;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Domain.Handlers;
    using SpreadBet.Infrastructure.Messaging.Handlers;
    using SpreadBet.Repository;

    [TestClass]
    public class CloseCommandHandlerTests
    {
        private ICommandHandler<CloseBetCommand> _handler;
        private Mock<IBetController> _mockBetController;
        private Mock<IPortfolioDataProvider> _portfolioDataProvider;
        private Fixture _fixture;

        [TestInitialize]
        public void SetUp()
        {
            _fixture = new Fixture();

            _mockBetController = new Mock<IBetController>();
            _portfolioDataProvider = new Mock<IPortfolioDataProvider>();
            _handler = new CloseCommandHandler(_portfolioDataProvider.Object, 
                                                _mockBetController.Object);
        }

        [TestMethod]
        public void Execute_BetAndStockValid_BetPlacedAndStored()
        {
            // Arrange
            var cmd = _fixture.Create<CloseBetCommand>();

            _mockBetController.Setup(x => x.Close(cmd.Bet)).Returns(true);

            // Act
            _handler.Handle(cmd);

            // Assert
            _mockBetController.Verify(x => x.Close(cmd.Bet), Times.Once());
            _portfolioDataProvider.Verify(x => x.SaveBet(cmd.Bet), Times.Once());
        }
    }
}
