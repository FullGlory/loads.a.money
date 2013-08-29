using System;
using Moq;
using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Infrastructure;
using SpreadBet.Infrastructure.Messaging;
using SpreadBet.Scheduler;

namespace SpreadBet.MarketData.UnitTests
{
    [TestFixture]
    public class PriceFeedBotProcessorTests
    {
        private IProcessor _processor;
        private Mock<IScheduler> _mockScheduler;
        private Mock<IStockPriceBot> _mockStockPriceBot;
        private Mock<ISender<StockPrice>> _mockSender;
        private string _schedulerConfig;

        [SetUp]
        public void SetUp()
        {
            _mockScheduler = new Mock<IScheduler>();
            _mockStockPriceBot = new Mock<IStockPriceBot>();
            _mockSender = new Mock<ISender<StockPrice>>();
            _schedulerConfig = "00:01:10";
            _processor = new PriceFeedBotProcessor(_mockScheduler.Object, _mockStockPriceBot.Object, _mockSender.Object, _schedulerConfig);
        }

        [Test]
        public void StartInitialisesScheduler()
        {
            // Arrange

            // Act
            _processor.Start();

            // Assert
            _mockScheduler.Verify(x => x.AddScheduledAction(It.IsAny<ScheduledActiondDelegate>(), It.IsAny<TimeSpan>()), Times.Once());
            _mockScheduler.Verify(x => x.Start(), Times.Once());
        }

        [Test]
        public void SchedulerConfiguredFromTimeSpanString()
        {
            // Arrange

            // Act
            _processor.Start();

            // Assert
            _mockScheduler.Verify(x => x.AddScheduledAction(It.IsAny<ScheduledActiondDelegate>(), It.Is<TimeSpan>(ts => ts.TotalSeconds == 70)), Times.Once());
        }

        [Test]
        public void SchedulerInitialisedOnlyOnce()
        {
            // Arrange

            // Act
            _processor.Start();
            _processor.Stop();
            _processor.Start();

            // Assert
            _mockScheduler.Verify(x => x.AddScheduledAction(It.IsAny<ScheduledActiondDelegate>(), It.IsAny<TimeSpan>()), Times.Once());
        }

        [Test]
        public void StopSuspendsScheduler()
        {
            // Arrange

            // Act
            _processor.Stop();

            // Assert
            _mockScheduler.Verify(x => x.Stop(), Times.Once());
        }

        [Test]
        public void BotInvokedAndSchedulerRestartedWhenSchedulerJobTriggered()
        {
            // Arrange
            var sp = new StockPrice();

            // When AddScheduledAction called, immediately invoke the supplied delegate as supplied by the class
            _mockScheduler.Setup(x => x.AddScheduledAction(It.IsAny<ScheduledActiondDelegate>(), It.IsAny<TimeSpan>()))
                          .Callback<ScheduledActiondDelegate,TimeSpan>((d,t) => d());

            // In the ScheduledActiondDelegate we want the bot to scrape and the scheduler to be restarted
            _mockStockPriceBot.Setup(x => x.Scrape(It.IsAny<Action<StockPrice>>()))
                              .Callback<Action<StockPrice>>((action) => action(sp));

            // Act
            _processor.Start();

            // Assert
            _mockStockPriceBot.Verify(x => x.Scrape(It.IsAny<Action<StockPrice>>()), Times.Once());
            _mockScheduler.Verify(x => x.Start(), Times.Once());
            _mockSender.Verify(x => x.Send(sp), Times.Once());
        }
    }
}
