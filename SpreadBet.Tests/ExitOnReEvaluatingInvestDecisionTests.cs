using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpreadBet.Common.Entities;
using SpreadBet.Common.Interfaces;
using Moq;
using SpreadBet.Common.Components;
using SpreadBet.Domain;

namespace SpreadBet.Tests
{
	[TestClass]
	public class ExitOnReEvaluatingInvestDecisionTests
	{
		[TestMethod]
		public void GetExitDecision_ExistingBetsProvider_CallsInvestDeciderForEachExistingStock()
		{
			var bets = new Bet[]
			{
				new Bet { Stock = new Stock { Identifier = "STK.1" } }, 
				new Bet { Stock = new Stock { Identifier = "STK.2" } }
			};

			var stocks = bets.Select(bet => bet.Stock);

			var investDecider = new Mock<IInvestDecider>();
			investDecider.Setup(x => x.GetInvestmentDescisions(It.IsAny<IEnumerable<Stock>>())).Returns(bets);
 
			var exitDecider = new ExitOnReEvaluatingInvestDecision(investDecider.Object);

			var results = exitDecider.GetExitDescisions(bets);

			investDecider.Verify(x => x.GetInvestmentDescisions(stocks), Times.Once());
		}

		[TestMethod]
		public void GetExitDecision_StockInProfitStartsToDecreaseAgainstPosition_ReturnsExitDecisions()
		{
			var stk1 = new Stock
			{
				Identifier = "STK.1"
			};

			var stocks = new Stock[] { stk1 };

			var stk1Bet_old = new Bet
			{
				Stock = stk1,
				BidAmount = 100m,
				Direction = Direction.Increase,
				OpeningPosition = 90.00m,
				Price = new Price
				{
					Offer = 80.00m,
					Bid = 90.00m
				}
			};

			var stk1Bet_new = new Bet
			{
				Stock = stk1,
				BidAmount = 100m,
				Direction = Direction.Decrease,
				OpeningPosition = 100.00m,
				Price = new Price
				{
					Offer = 100.00m,
					Bid = 110.00m
				}
			};

			var newBets = new Bet[] { stk1Bet_new };
			var oldBets = new Bet[] { stk1Bet_old };

			var investDecider = new Mock<IInvestDecider>();
			investDecider.Setup(x => x.GetInvestmentDescisions(stocks)).Returns(newBets);

			var exitDecider = new ExitOnReEvaluatingInvestDecision(investDecider.Object);
			var results = exitDecider.GetExitDescisions(oldBets);

			Assert.AreEqual(1, results.Count());
			Assert.AreEqual(stk1Bet_old, results.Single());
		}

		[TestMethod]
		public void GetExitDecision_StockInProfitStartsToIncreaseAgainstPosition_ReturnsExitDecisions()
		{
			var stk1 = new Stock
			{
				Identifier = "STK.1"
			};

			var stocks = new Stock[] { stk1 };

			var stk1Bet_old = new Bet
			{
				Stock = stk1,
				BidAmount = 100m,
				Direction = Direction.Decrease,
				OpeningPosition = 80.00m,
				Price = new Price
				{
					Offer = 80.00m,
					Bid = 90.00m
				}
			};

			var stk1Bet_new = new Bet
			{
				Stock = stk1,
				BidAmount = 100m,
				Direction = Direction.Increase,
				OpeningPosition = 100.00m,
				Price = new Price
				{
					Offer = 65.00m,
					Bid = 75.00m
				}
			};

			var newBets = new Bet[] { stk1Bet_new };
			var oldBets = new Bet[] { stk1Bet_old };

			var investDecider = new Mock<IInvestDecider>();
			investDecider.Setup(x => x.GetInvestmentDescisions(stocks)).Returns(newBets);

			var exitDecider = new ExitOnReEvaluatingInvestDecision(investDecider.Object);
			var results = exitDecider.GetExitDescisions(oldBets);

			Assert.AreEqual(1, results.Count());
			Assert.AreEqual(stk1Bet_old, results.Single());
		}

		[TestMethod]
		public void GetExitDecision_StockNotInProfitStartsToDecreaseAgainstPosition_ReturnsExitDecisions()
		{
			var stk1 = new Stock
			{
				Identifier = "STK.1"
			};

			var stocks = new Stock[] { stk1 };

			var stk1Bet_old = new Bet
			{
				Stock = stk1,
				BidAmount = 100m,
				Direction = Direction.Increase,
				OpeningPosition = 90.00m,
				Price = new Price
				{
					Offer = 80.00m,
					Bid = 90.00m
				}
			};

			var stk1Bet_new = new Bet
			{
				Stock = stk1,
				BidAmount = 100m,
				Direction = Direction.Decrease,
				OpeningPosition = 100.00m,
				Price = new Price
				{
					Offer = 89.90m,
					Bid = 99.90m
				}
			};

			var newBets = new Bet[] { stk1Bet_new };
			var oldBets = new Bet[] { stk1Bet_old };

			var investDecider = new Mock<IInvestDecider>();
			investDecider.Setup(x => x.GetInvestmentDescisions(stocks)).Returns(newBets);

			var exitDecider = new ExitOnReEvaluatingInvestDecision(investDecider.Object);
			var results = exitDecider.GetExitDescisions(oldBets);

			Assert.AreEqual(0, results.Count());
		}
	}
}
