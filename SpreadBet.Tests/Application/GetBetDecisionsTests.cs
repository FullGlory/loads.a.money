using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Ploeh.AutoFixture;
using SpreadBet.Application;
using SpreadBet.CommandBus;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Domain.Commands;

namespace SpreadBet.Tests.Application
{
    [TestClass]
    public class GetBetDecisionsTests
    {
        private IExecutableApplication _app;
        private Mock<IStockDataProvider> _mockStockDataProvider;
        private Mock<IInvestDecider> _mockInvestDecider;
        private Mock<IStockFilter> _mockStockFilter;
        private Mock<ICommandSender> _mockCommandBus;
        private Mock<IUpdate> _mockPriceUpdate;
        private Fixture _fixture;

        [TestInitialize]
        public void SetUp()
        {
            _mockStockDataProvider = new Mock<IStockDataProvider>();
            _mockInvestDecider = new Mock<IInvestDecider>();
            _mockStockFilter = new Mock<IStockFilter>();
            _mockCommandBus = new Mock<ICommandSender>();
            _mockPriceUpdate = new Mock<IUpdate>();
            _app = new GetBetDecisions(
                _mockStockDataProvider.Object,
                _mockStockFilter.Object, 
                _mockInvestDecider.Object, 
                _mockCommandBus.Object,
                _mockPriceUpdate.Object);

            _fixture = new Fixture();
        }

        [TestMethod]
        public void Run_BetOpportunityForSingleStock_BetPlaced()
        {
            // Arrange
            var stock = _fixture.Create<Stock>();
            var stocks = new List<Stock> { stock };
            var bet = _fixture.Build<Bet>()
                              .With(x=>x.Stock, stock)
                              .Create();

            _mockStockDataProvider.Setup(x => x.GetStocks()).Returns(stocks);
            _mockStockFilter.Setup(x => x.GetInvestmentCandidates(stocks)).Returns(stocks);
            _mockInvestDecider.Setup(x => x.GetInvestmentDescisions(stocks)).Returns(new List<Bet> { bet });   

            // Act
            _app.Run();

            // Assert
            _mockStockDataProvider.VerifyAll();
            _mockStockFilter.VerifyAll();
            _mockInvestDecider.VerifyAll();

            _mockCommandBus.Verify(
                    x => x.Send(
                        It.Is<PlaceBetCommand>(
                            cmd => cmd.StockIdentifier == bet.Stock.Identifier &&
                                cmd.InitialLoss == bet.InitialLoss &&
                                cmd.BidAmount == bet.BidAmount &&
                                cmd.OpeningPosition == bet.OpeningPosition &&
                                cmd.ExitPrice == bet.ExitPrice &&
                                cmd.IsIncrease == (bet.Direction == Direction.Increase)
                                                )
                        ), 
                    Times.Once());
        }


    }
}
