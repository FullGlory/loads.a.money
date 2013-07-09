﻿namespace SpreadBet.Tests.Domain.CommandHandlers
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ploeh.AutoFixture;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Domain.Handlers;
    using SpreadBet.Infrastructure.Messaging.Handlers;

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
            var bet = _fixture.Create<Bet>();
            var cmd = _fixture.Build<CloseBetCommand>()
                              .With(x=>x.BetId, bet.Id)
                              .Create();
            
            _portfolioDataProvider.Setup(x=>x.Get(bet.Id)).Returns(bet);

            _mockBetController.Setup(x => x.Close(bet)).Returns(true);

            // Act
            _handler.Handle(cmd);

            // Assert
            _portfolioDataProvider.VerifyAll();
            _mockBetController.Verify(x => x.Close(bet), Times.Once());
            _portfolioDataProvider.Verify(x => x.SaveBet(bet), Times.Once());
        }
    }
}
