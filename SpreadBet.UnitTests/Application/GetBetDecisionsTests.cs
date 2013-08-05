namespace SpreadBet.UnitTests.Application
{
    using System.Collections.Generic;
    using NUnit.Framework;
    using Moq;
    using Ploeh.AutoFixture;
    using SpreadBet.Application;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Infrastructure;
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure.Serialisation;

    [TestFixture]
    public class GetBetDecisionsTests
    {
        private IProcessor _app;
        private ICommandBus _commandBus;
        private Mock<IStockDataProvider> _mockStockDataProvider;
        private Mock<IInvestDecider> _mockInvestDecider;
        private Mock<IStockFilter> _mockStockFilter;
        private Mock<IMessageSender> _mockMessageSender;
        private Mock<ITextSerialiser> _mockTextSerialiser;
        private Mock<IUpdate> _mockPriceUpdate;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _mockStockDataProvider = new Mock<IStockDataProvider>();
            _mockInvestDecider = new Mock<IInvestDecider>();
            _mockStockFilter = new Mock<IStockFilter>();
            _mockPriceUpdate = new Mock<IUpdate>();
            _mockMessageSender = new Mock<IMessageSender>();
            _mockTextSerialiser = new Mock<ITextSerialiser>();

            _commandBus = new CommandBus(_mockMessageSender.Object, _mockTextSerialiser.Object);
            _app = new GetBetDecisions(
                _mockStockDataProvider.Object,
                _mockStockFilter.Object, 
                _mockInvestDecider.Object,
                _commandBus,
                _mockPriceUpdate.Object);

            _fixture = new Fixture();
        }

        [Test]
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
            _mockPriceUpdate.Setup(x => x.Update(stocks));
            _mockStockFilter.Setup(x => x.GetInvestmentCandidates(stocks)).Returns(stocks);
            _mockInvestDecider.Setup(x => x.GetInvestmentDescisions(stocks)).Returns(new List<Bet> { bet });
            _mockMessageSender.Setup(x => x.Send(It.IsAny<Message>()));

            // Act
            _app.Start();

            // Assert
            _mockStockDataProvider.VerifyAll();
            _mockStockFilter.VerifyAll();
            _mockPriceUpdate.VerifyAll();
            _mockInvestDecider.VerifyAll();
            _mockMessageSender.VerifyAll();


        }


    }
}
