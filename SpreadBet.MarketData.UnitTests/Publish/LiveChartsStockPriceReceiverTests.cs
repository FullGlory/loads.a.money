using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Moq;
using SpreadBet.Domain;
using SpreadBet.MarketData.Publish;
using SpreadBet.Scheduler;
using SpreadBet.Common.Interfaces;

namespace SpreadBet.MarketData.UnitTests.Publish
{
    [TestFixture]
    public class LiveChartsStockPriceReceiverTests
    {
        private IReceiver<StockPrice> _publisher;
        private Mock<IScheduler> _mockScheduler;
        private Mock<IScraper> _mockScraper;
        private Mock<IStockMarket> _mockStockMarket;

        [SetUp]
        public void SetUp()
        {
            _mockScheduler = new Mock<IScheduler>();
            _mockScraper = new Mock<IScraper>();
            _mockStockMarket = new Mock<IStockMarket>();
            _publisher = new LiveChartsStockPriceReceiver(_mockScheduler.Object, _mockStockMarket.Object, _mockScraper.Object);
        }

        [Test]
        public void StartScheduler()
        {
            // Arrange

            // Act
            _publisher.Start((stockPrice) => {});

            // Assert
            _mockScheduler.Verify(x => x.AddScheduledAction(It.IsAny<ScheduledActiondDelegate>(), It.IsAny<TimeSpan>()), Times.Once());
            _mockScheduler.Verify(x => x.Start(), Times.Once());
        }

        [Test]
        public void ScheduledActionOnlySetupOnce()
        {
            // Arrange
            _publisher.Start((stockPrice) => { });

            // Act
            _publisher.Start((stockPrice) => { });

            // Assert
            _mockScheduler.Verify(x => x.AddScheduledAction(It.IsAny<ScheduledActiondDelegate>(), It.IsAny<TimeSpan>()), Times.Once());
        }

        [Test]
        public void StopScheduler()
        {
            // Arrange

            // Act
            _publisher.Stop();

            // Assert
            _mockScheduler.Verify(x => x.Stop(), Times.Once());
        }
    }
}
