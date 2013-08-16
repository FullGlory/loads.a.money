using System.Collections.Generic;
using System.Linq;
using Moq;
using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.MarketData.Publish;

namespace SpreadBet.MarketData.UnitTests.Publish
{
    [TestFixture]
    public class StockPricePublisherTests
    {
        private Mock<IMessageSender> _mockOutputChannel;
        private Mock<IMarket> _mockMarket;
        private Mock<IStockList> _mockStockList;
        private Mock<IPriceGateway> _mockPriceGateway;
        private StockPricePublisher _feed;

        [SetUp]
        public void SetUp()
        {
            _mockOutputChannel = new Mock<IMessageSender>();
            _mockMarket = new Mock<IMarket>();
            _mockStockList = new Mock<IStockList>();
            _mockPriceGateway = new Mock<IPriceGateway>();

            _feed = new StockPricePublisher(
                _mockMarket.Object, 
                _mockStockList.Object, 
                _mockPriceGateway.Object, 
                _mockOutputChannel.Object);
        }

        [Test]
        public void PublishesStockPriceData()
        {
            // Arrange
            _mockMarket.Setup(x => x.IsOpen).Returns(true);
            var seedIdentifier = "RTR.L";
            _mockStockList.Setup(x => x.GetStocks()).Returns(new List<Stock> { new Stock { Identifier = seedIdentifier } }.AsEnumerable());
            var seedPrice = 69.0M;
            _mockPriceGateway.Setup(x => x.GetStockPrice(seedIdentifier)).Returns(new Price { Bid = seedPrice });

            // Act
            _feed.Publish();

            // Assert
            _mockMarket.VerifyAll();
            _mockStockList.VerifyAll();
            _mockPriceGateway.VerifyAll();

            // TODO - decide to use channel adapter or envelope??
            _mockOutputChannel.Verify(x=>x.Send(It.IsAny<Message>()), Times.Once());
        }
    }
}
