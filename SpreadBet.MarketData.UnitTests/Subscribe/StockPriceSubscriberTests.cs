using Moq;
using NUnit.Framework;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.MarketData.Subscribe;

namespace SpreadBet.MarketData.UnitTests.Subscribe
{
    [TestFixture]
    public class StockPriceSubscriberTests
    {
        private Mock<IMessageReceiver> _mockInputChannel;
        private Mock<IStockDataProvider> _mockRepository;
        private StockPriceSubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _mockInputChannel = new Mock<IMessageReceiver>();
            _mockRepository = new Mock<IStockDataProvider>();
            _subscriber = new StockPriceSubscriber(_mockInputChannel.Object, _mockRepository.Object);
        }

        [Test]
        public void StartListeningForStockPriceUpdatesWhenStartIsCalled()
        {
            // Arrange

            // Act
            _subscriber.Start();

            // Assert
            _mockInputChannel.Verify(x => x.Start());
        }

        [Test]
        public void StoreStockPriceWhenUpdateReceived()
        {
            // Arrange
            _subscriber.Start();
            var seedMsg = new Message();

            // Act
            _mockInputChannel.Raise(x => x.MessageReceived += null, new MessageReceivedEventArgs(seedMsg));

            // Assert
            // TODO - work out how to unpack message into stockprice
            _mockRepository.Verify(x => x.AddStockPrice(It.IsAny<StockPrice>()), Times.Once());
        }
    }
}
