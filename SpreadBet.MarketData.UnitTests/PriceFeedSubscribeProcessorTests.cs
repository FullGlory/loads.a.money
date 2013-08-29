using System;
using Moq;
using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Domain.Interfaces;
using SpreadBet.Infrastructure;
using SpreadBet.Infrastructure.Messaging;

namespace SpreadBet.MarketData.UnitTests
{
    [TestFixture]
    public class PriceFeedSubscribeProcessorTests
    {
        private IProcessor _processor;
        private Mock<IReceiver<StockPrice>> _mockReceiver;
        private Mock<IStockPriceRepository> _mockStockPriceRepository;

        [SetUp]
        public void SetUp()
        {
            _mockReceiver = new Mock<IReceiver<StockPrice>>();
            _mockStockPriceRepository = new Mock<IStockPriceRepository>();
            _processor = new PriceFeedSubscribeProcessor(_mockReceiver.Object, _mockStockPriceRepository.Object);
        }

        [Test]
        public void StartRegistersDelegateWithReceiver()
        {
            // Arrange

            // Act
            _processor.Start();

            // Assert
            _mockReceiver.Verify(x => x.Start(It.IsAny<Action<StockPrice>>()), Times.Once());
        }

        [Test]
        public void StopReceiver()
        {
            // Arrange

            // Act
            _processor.Stop();

            // Assert
            _mockReceiver.Verify(x => x.Stop(), Times.Once());
        }

        [Test]
        public void ReceivedStockPriceForwardedToRepository()
        {
            // Arrange
            var sp = new StockPrice();

            _mockReceiver.Setup(x => x.Start(It.IsAny<Action<StockPrice>>()))
                         .Callback<Action<StockPrice>>((action) => action(sp));

            // Act
            _processor.Start();

            // Assert
            _mockStockPriceRepository.Verify(x => x.AddStockPrice(sp), Times.Once());
        }
    }
}
