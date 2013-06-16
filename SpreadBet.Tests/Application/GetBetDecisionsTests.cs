using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpreadBet.Application;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;

namespace SpreadBet.Tests.Application
{
    [TestClass]
    public class GetBetDecisionsTests
    {
        private IExecutableApplication _app;
        private Mock<IStockDataProvider> _mockStockDataProvider;
        private Mock<IInvestDecider> _mockInvestDecider;
        private Mock<IStockFilter> _mockStockFilter;
        private Mock<IPortfolioDataProvider> _mockPortfolioDataProvider;
        private Mock<IAccountDataProvider> _mockAccountDataProvider;
        private Mock<IBetController> _mockBetController;

        [TestInitialize]
        public void SetUp()
        {
            _mockStockDataProvider = new Mock<IStockDataProvider>();
            _mockInvestDecider = new Mock<IInvestDecider>();
            _mockStockFilter = new Mock<IStockFilter>();
            _mockPortfolioDataProvider = new Mock<IPortfolioDataProvider>();
            _mockAccountDataProvider = new Mock<IAccountDataProvider>();
            _mockBetController = new Mock<IBetController>();
            _app = new GetBetDecisions(
                _mockStockDataProvider.Object, 
                _mockInvestDecider.Object, 
                _mockStockFilter.Object, 
                _mockPortfolioDataProvider.Object,
                _mockAccountDataProvider.Object,
                _mockBetController.Object);
        }

        [TestMethod]
        public void Run_BetOpportunityForSingleStock_BetPlaced()
        {
            // Arrange
            var stock = new Stock();
            var stocks = new List<Stock> { stock };
            var bet = new Bet();
            var account = new Account();

            _mockStockDataProvider.Setup(x => x.GetStocks()).Returns(stocks);
            _mockStockFilter.Setup(x => x.GetInvestmentCandidates(stocks)).Returns(stocks);
            _mockInvestDecider.Setup(x => x.GetInvestmentDescisions(stocks)).Returns(new List<Bet> { bet });
            _mockAccountDataProvider.Setup(x => x.GetCurrentPosition()).Returns(account);
            _mockBetController.Setup(x => x.Open(account, bet)).Returns(true);

            // Act
            _app.Run();

            // Assert
            _mockStockDataProvider.VerifyAll();
            _mockStockFilter.VerifyAll();
            _mockInvestDecider.VerifyAll();
            _mockAccountDataProvider.VerifyAll();
            _mockBetController.VerifyAll();
            _mockPortfolioDataProvider.Verify(x => x.SaveBet(bet), Times.Once());
        }


    }
}
