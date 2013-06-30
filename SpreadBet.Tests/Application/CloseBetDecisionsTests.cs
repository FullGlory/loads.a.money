namespace SpreadBet.Tests.Application
{
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Ploeh.AutoFixture;
    using SpreadBet.Application;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Infrastructure;
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure.Serialisation;

    [TestClass]
    public class CloseBetDecisionsTests
    {
        private IProcessor _app;
        private ICommandBus _commandBus;
        private Mock<IPortfolioDataProvider> _mockPortfolioDataProvider;
        private Mock<IExitDecider> _mockExitDecider;
        private Mock<IMessageSender> _mockMessageSender;
        private Mock<ITextSerialiser> _mockTextSerialiser;
        private Mock<IUpdate> _mockPriceUpdate;
        private Fixture _fixture;

        [TestInitialize]
        public void SetUp()
        {
            _mockPortfolioDataProvider = new Mock<IPortfolioDataProvider>();
            _mockExitDecider = new Mock<IExitDecider>();
            _mockMessageSender = new Mock<IMessageSender>();
            _mockTextSerialiser = new Mock<ITextSerialiser>();
            _mockPriceUpdate = new Mock<IUpdate>();

            _commandBus = new CommandBus(_mockMessageSender.Object, _mockTextSerialiser.Object);
            _app = new CloseBetDecisions(
                _mockPortfolioDataProvider.Object,
                _mockExitDecider.Object,
                _commandBus,
                _mockPriceUpdate.Object);

            _fixture = new Fixture();
        }

        [TestMethod]
        public void Run_BetOpportunityForSingleStock_BetClosed()
        {
            // Arrange
            var bet = _fixture.Create<Bet>();
            var bets = new List<Bet> { bet };

            _mockPortfolioDataProvider.Setup(x => x.GetCurrentBets()).Returns(bets);
            _mockPriceUpdate.Setup(x => x.Update(bets));
            _mockExitDecider.Setup(x => x.GetExitDescisions(bets)).Returns(bets);
            _mockMessageSender.Setup(x => x.Send(It.IsAny<Message>()));

            // Act
            _app.Start();

            // Assert
            _mockPortfolioDataProvider.VerifyAll();
            _mockPriceUpdate.VerifyAll();
            _mockExitDecider.VerifyAll();
            _mockMessageSender.VerifyAll();
        }


    }
}
