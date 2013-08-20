using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpreadBet.Infrastructure;
using Moq;
using SpreadBet.Domain;
using SpreadBet.MarketData.Endpoint;
using SpreadBet.MarketData;

namespace SpreadBet.MarketData.UnitTests.Endpoint
{
    [TestFixture]
    public class PriceFeedProcessorTests
    {
        private IProcessor _processor;
        private Mock<IReceiver<StockPrice>> _mockReceiver;
        private Mock<ISender<StockPrice>> _mockSender;

        [SetUp]
        public void SetUp()
        {
            _mockReceiver = new Mock<IReceiver<StockPrice>>();
            _mockSender = new Mock<ISender<StockPrice>>();
            _processor = new PriceFeedProcessor(_mockReceiver.Object, _mockSender.Object);
        }

        [Test]
        public void StartReceivingStockPrices()
        {
            // Arrange

            // Act
            _processor.Start();

            // Assert
            _mockReceiver.Verify(x=>x.Start(It.IsAny<Action<StockPrice>>()), Times.Once());
        }

        [Test]
        public void StopReceivingStockPrices()
        {
            // Arrange

            // Act
            _processor.Stop();

            // Assert
            _mockReceiver.Verify(x => x.Stop(), Times.Once());
        }
    }
}
