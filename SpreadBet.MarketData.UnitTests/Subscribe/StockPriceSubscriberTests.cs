using Moq;
using NUnit.Framework;
using SpreadBet.Common.Interfaces;
using SpreadBet.Domain;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.Infrastructure.Serialisation;
using SpreadBet.MarketData.Subscribe;
using System;

namespace SpreadBet.MarketData.UnitTests.Subscribe
{
    [TestFixture]
    public class StockPriceSubscriberTests
    {
        private Mock<ITextSerialiser> _mockTextSerialiser;
        private Mock<IMessageReceiver> _mockInputChannel;
        private Mock<IMessageSender> _mockDeadLetterChannel;
        private Mock<IStockDataProvider> _mockRepository;
        private StockPriceSubscriber _subscriber;

        [SetUp]
        public void SetUp()
        {
          _mockTextSerialiser = new Mock<ITextSerialiser>();
            _mockInputChannel = new Mock<IMessageReceiver>();
            _mockDeadLetterChannel = new Mock<IMessageSender>();
            _mockRepository = new Mock<IStockDataProvider>();
            _subscriber = new StockPriceSubscriber(_mockTextSerialiser.Object, _mockInputChannel.Object, _mockRepository.Object, _mockDeadLetterChannel.Object);
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
            
            var seedMsgData = Guid.NewGuid().ToString();
            var seedMsg = new Message
            {
              Body = seedMsgData
            };
            var seedStockPrice = new StockPrice();
            _mockTextSerialiser.Setup(x => x.Deserialize(seedMsgData)).Returns(seedStockPrice);

            // Act
            _mockInputChannel.Raise(x => x.MessageReceived += null, new MessageReceivedEventArgs(seedMsg));

            // Assert
            // TODO - work out how to unpack message into stockprice
            _mockRepository.Verify(x => x.AddStockPrice(seedStockPrice), Times.Once());
            _mockTextSerialiser.VerifyAll();
        }

        [Test]
        public void MovesStockPriceToDeadLetterQueueWhenAttemptToStoreUpdateFails()
        {
          // Arrange
          _subscriber.Start();
          var seedMsgData = Guid.NewGuid().ToString();
          var seedMsg = new Message
          {
            Body = seedMsgData
          };
          var seedStockPrice = new StockPrice();
          _mockTextSerialiser.Setup(x => x.Deserialize(seedMsgData)).Returns(seedStockPrice);

          _mockRepository.Setup(x => x.AddStockPrice(It.IsAny<StockPrice>())).Throws<Exception>();

          // Act
          _mockInputChannel.Raise(x => x.MessageReceived += null, new MessageReceivedEventArgs(seedMsg));

          // Assert
          // TODO - work out how to unpack message into stockprice
          _mockDeadLetterChannel.Verify(x => x.Send(seedMsg), Times.Once());
          _mockTextSerialiser.VerifyAll();
        }
    }
}
