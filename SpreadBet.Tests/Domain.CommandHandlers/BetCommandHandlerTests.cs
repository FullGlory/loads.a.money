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
    public class BetCommandHandlerTests
    {
        private ICommandHandler<PlaceBetCommand> _handler;
        private Mock<IBetController> _mockBetController;
        private Mock<IAccountDataProvider> _mockAccountDataProvider;
        private Mock<IStockDataProvider> _stockDataProvider;
        private Mock<IPortfolioDataProvider> _portfolioDataProvider;
        private Fixture _fixture;

        [TestInitialize]
        public void SetUp()
        {
            _fixture = new Fixture();

            _mockBetController = new Mock<IBetController>();
            _mockAccountDataProvider = new Mock<IAccountDataProvider>();
            _stockDataProvider = new Mock<IStockDataProvider>();
            _portfolioDataProvider = new Mock<IPortfolioDataProvider>();
            _handler = new BetCommandHandler(_portfolioDataProvider.Object, 
                                                _mockAccountDataProvider.Object,
                                                _stockDataProvider.Object, 
                                                _mockBetController.Object);
        }

        [TestMethod]
        public void Execute_BetAndStockValid_BetPlacedAndStored()
        {
            // Arrange
            var cmd = _fixture.Create<PlaceBetCommand>();
            var account = _fixture.Create<Account>();
            var stock = _fixture.Build<Stock>().With(s=>s.Identifier, cmd.StockIdentifier).Create();
            
            _mockAccountDataProvider.Setup(x => x.GetCurrentPosition()).Returns(account);
            _stockDataProvider.Setup(x => x.GetStock(cmd.StockIdentifier)).Returns(stock);
            _mockBetController.Setup(x => x.Open(It.Is<Bet>(b => b.Stock.Identifier == cmd.StockIdentifier && b.BidAmount == cmd.BidAmount && b.ExitPrice == cmd.ExitPrice && b.InitialLoss == cmd.InitialLoss && b.OpeningPosition == cmd.OpeningPosition))).Returns(true);

            // Act
            _handler.Handle(cmd);

            // Assert
            _mockAccountDataProvider.VerifyAll();
            _stockDataProvider.VerifyAll();
            _mockBetController.VerifyAll();
            _portfolioDataProvider.Verify(x => x.SaveBet(It.Is<Bet>(b => b.Stock.Identifier == cmd.StockIdentifier && b.BidAmount == cmd.BidAmount && b.ExitPrice == cmd.ExitPrice && b.InitialLoss == cmd.InitialLoss && b.OpeningPosition == cmd.OpeningPosition)), Times.Once());
        }
    }
}
