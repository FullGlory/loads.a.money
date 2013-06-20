using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using SpreadBet.Common.Components;
using SpreadBet.Domain;

namespace SpreadBet.Tests
{
	[TestClass]
	public class CompositeExitDeciderTests
	{
		[TestMethod]
		public void GetExitDecisions_NoExitDecisionsReached_ExecutesAlLFilters()
		{
			var bets = new Bet[]
			{
				new Bet { Id = 1, PlacedOn = DateTime.UtcNow.AddHours(-2), BidAmount = 13.12m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.1" } }, 
				new Bet { Id = 1, PlacedOn = DateTime.UtcNow.AddHours(-1), BidAmount = 13.13m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.2" } }, 
			};

			var decider1 = new Mock<IExitDecider>();
			decider1.Setup(x => x.GetExitDescisions(bets)).Returns(new List<Bet>());

			var decider2 = new Mock<IExitDecider>();
			decider2.Setup(x => x.GetExitDescisions(bets)).Returns(new List<Bet>());

			var decider3 = new Mock<IExitDecider>();
			decider3.Setup(x => x.GetExitDescisions(bets)).Returns(new List<Bet>());

			var compositeFilter = new CompositeExitDecider(decider1.Object, decider2.Object, decider3.Object);

			var result = compositeFilter.GetExitDescisions(bets);

			decider1.Verify(x => x.GetExitDescisions(It.IsAny<IEnumerable<Bet>>()), Times.Once());
			decider2.Verify(x => x.GetExitDescisions(It.IsAny<IEnumerable<Bet>>()), Times.Once());
			decider3.Verify(x => x.GetExitDescisions(It.IsAny<IEnumerable<Bet>>()), Times.Once());

			Assert.AreEqual(0, result.Count());
			Assert.AreEqual(2, bets.Count());
            Assert.IsTrue(bets.Any(x => x.Stock.Identifier.Equals("STK.1")));
			Assert.IsTrue(bets.Any(x => x.Stock.Identifier.Equals("STK.2")));
		}

		[TestMethod]
		public void GetExitDecisions_ExitDecisionsReached_DoesNotLaunchUnnecessaryDeciders()
		{
			var bets = new Bet[]
			{
				new Bet { Id = 1, BidAmount = 13.12m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.1" } }, 
				new Bet { Id = 1, BidAmount = 13.13m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.2" } }, 
			};

			var decider1 = new Mock<IExitDecider>();
			decider1.Setup(x => x.GetExitDescisions(bets)).Returns(new List<Bet>());

			var decider2 = new Mock<IExitDecider>();
			decider2.Setup(x => x.GetExitDescisions(bets)).Returns(bets); // This stage will decide to dump all of our existing bets

			var decider3 = new Mock<IExitDecider>();
			decider3.Setup(x => x.GetExitDescisions(bets)).Returns(new List<Bet>());	// This stage will therefore not run because there are no bets left

			var compositeFilter = new CompositeExitDecider(decider1.Object, decider2.Object, decider3.Object);

			var result = compositeFilter.GetExitDescisions(bets);

			decider1.Verify(x => x.GetExitDescisions(It.IsAny<IEnumerable<Bet>>()), Times.Once());
			decider2.Verify(x => x.GetExitDescisions(It.IsAny<IEnumerable<Bet>>()), Times.Once());
			decider3.Verify(x => x.GetExitDescisions(It.IsAny<IEnumerable<Bet>>()), Times.Never());
		}
	}
}
