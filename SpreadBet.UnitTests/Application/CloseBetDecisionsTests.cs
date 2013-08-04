namespace SpreadBet.Tests.Application
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
    public class CloseBetDecisionsTests
    {
        private IProcessor _app;
        private Mock<ICommandBus> _mockCommandBus;
        private Mock<IPortfolioDataProvider> _mockPortfolioDataProvider;
        private Mock<IExitDecider> _mockExitDecider;
        private Mock<IUpdate> _mockPriceUpdate;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _mockPortfolioDataProvider = new Mock<IPortfolioDataProvider>();
            _mockExitDecider = new Mock<IExitDecider>();
            _mockPriceUpdate = new Mock<IUpdate>();
            _mockCommandBus = new Mock<ICommandBus>();

            _app = new CloseBetDecisions(
                _mockPortfolioDataProvider.Object,
                _mockExitDecider.Object,
                _mockCommandBus.Object,
                _mockPriceUpdate.Object);

            _fixture = new Fixture();
        }

        [Test]
        public void Run_BetOpportunityForSingleStock_BetClosed()
        {
            // Arrange
            var bet = _fixture.Create<Bet>();
            var bets = new List<Bet> { bet };

            _mockPortfolioDataProvider.Setup(x => x.GetCurrentBets()).Returns(bets);
            _mockPriceUpdate.Setup(x => x.Update(bets));
            _mockExitDecider.Setup(x => x.GetExitDescisions(bets)).Returns(bets);

            // Act
            _app.Start();

            // Assert
            _mockPortfolioDataProvider.VerifyAll();
            _mockPriceUpdate.VerifyAll();
            _mockExitDecider.VerifyAll();
            _mockCommandBus.Verify(x => x.Send(It.Is<Envelope<ICommand>>(e => (e.Body as CloseBetCommand).BetId == bet.Id)), Times.Once());           
        }
    }
}
