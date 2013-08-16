using Moq;
using NUnit.Framework;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.MarketData.Subscribe;
using System;

namespace SpreadBet.MarketData.UnitTests.Subscribe
{
    [TestFixture]
    public class StockPriceSubscriberTests
    {
        private Mock<IMessageReceiver> _mockInputChannel;
        private Mock<IMessageSender> _mockDeadLetterChannel;
        private Mock<IStockDataProvider> _mockRepository;
        private StockPriceSubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
            _mockInputChannel = new Mock<IMessageReceiver>();
            _mockDeadLetterChannel = new Mock<IMessageSender>();
            _mockRepository = new Mock<IStockDataProvider>();
            _subscriber = new StockPriceSubscriber(_mockInputChannel.Object, _mockRepository.Object, _mockDeadLetterChannel.Object);
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

        [Test]
        public void MovesStockPriceToDeadLetterQueueWhenAttemptToStoreUpdateFails()
        {
          // Arrange
          _subscriber.Start();
          var seedMsg = new Message();

          _mockRepository.Setup(x => x.AddStockPrice(It.IsAny<StockPrice>())).Throws<Exception>();

          // Act
          _mockInputChannel.Raise(x => x.MessageReceived += null, new MessageReceivedEventArgs(seedMsg));

          // Assert
          // TODO - work out how to unpack message into stockprice
          _mockDeadLetterChannel.Verify(x => x.Send(seedMsg), Times.Once());
        }
    }
}
