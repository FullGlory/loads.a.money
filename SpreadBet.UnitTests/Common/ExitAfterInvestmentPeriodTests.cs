using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using SpreadBet.Domain;
using SpreadBet.Common.Components;

namespace SpreadBet.UnitTests.Common
{
	[TestFixture]
	public class ExitAfterInvestmentPeriodTests
	{
		[Test]
		public void GetExitDecisions_NoBetsAreWithinExitTimeFrame_ReturnsNoBetsToExit()
		{
			var bets = new Bet[]
			{
				new Bet { Id = 1, PlacedOn = DateTime.UtcNow.AddHours(-2), BidAmount = 13.12m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.1" } }, 
				new Bet { Id = 1, PlacedOn = DateTime.UtcNow.AddHours(-1), BidAmount = 13.13m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.2" } }, 
			};

			var exitDecider = new ExitAfterInvestmentPeriod(3);
			var result = exitDecider.GetExitDescisions(bets);

			Assert.AreEqual(0, result.Count());
		}

		[Test]
		public void GetExitDecisions_OneBetIsWithinExitTimeFrame_ReturnsBetToExit()
		{
			var bets = new Bet[]
			{
				new Bet { Id = 1, PlacedOn = DateTime.UtcNow.AddHours(-4), BidAmount = 13.12m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.1" } }, 
				new Bet { Id = 1, PlacedOn = DateTime.UtcNow.AddHours(-2), BidAmount = 13.13m, Direction = Direction.Increase, ExitPrice = 10.2m, InitialLoss = 100.01m, OpeningPosition = 12.5m, Stock = new Stock { Identifier = "STK.2" } }, 
			};

			var exitDecider = new ExitAfterInvestmentPeriod(3);
			var result = exitDecider.GetExitDescisions(bets);

			Assert.AreEqual(1, result.Count());
			Assert.AreEqual(bets.First(), result.First());
		}
	}
}
