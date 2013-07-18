namespace SpreadBet.Acceptance.Steps
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.Practices.Unity;
    using NUnit.Framework;
    using SpreadBet.Acceptance.Extensions;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using SpreadBet.Domain.Interfaces;
    using TechTalk.SpecFlow;

    [Binding]
    public class AutomationSteps
    {
        [Given(@"the valid account")]
        public void GivenTheValidAccount(Table table)
        {
            var provider = ScenarioContext.Current.Container().Resolve<IAutomationProvider>();
            var account = new Account { Id = 1, Username = table.Rows[0][0].ToString(), Password = table.Rows[0][1].ToString(), Deposit = 100000 };
            ScenarioContext.Current.Account(account);
            provider.Authenticate(account);
        }

        [Given(@"the following bets")]
        public void GivenTheFollowingBets(Table table)
        {
            var bets = new List<Bet>();

            foreach (var row in table.Rows)
            {
                var stock = new Stock { Identifier = row[0].ToString(), Name = row[1].ToString()};
                var bet = new Bet { Stock = stock, 
                                    Account = ScenarioContext.Current.Account(), 
                                    BidAmount = Convert.ToDecimal(row[2].ToString()), 
                                    Direction = (row[3].ToString().Equals("BUY")) ? Direction.Increase : Direction.Decrease };

                bets.Add(bet);
            }

            ScenarioContext.Current.Bets(bets);
        }

        [Then(@"(.*) the following bets")]
        public void ThenCloseTheFollowingBets(string type, Table table)
        {
            var controller = ScenarioContext.Current.Container().Resolve<IBetController>();
            var bets = ScenarioContext.Current.Bets();

            foreach (var bet in bets)
            {
                if (type.Equals("open"))
                {
                    Assert.IsTrue(controller.Open(bet));
                }
                else
                {
                    Assert.IsTrue(controller.Close(bet));
                }
            }
        }

        [Given(@"the following stocks")]
        public void GivenTheFollowingStocks(Table table)
        {
            var repo = ScenarioContext.Current.Container().Resolve<IRepository>();
            repo.SaveOrUpdate<Period>(new Period { From = DateTime.Now, To = DateTime.Now.AddHours(1)});

            foreach (var row in table.Rows)
            {
                var period = repo.GetAll<Period>().Last();
                var stock = new Stock { Identifier = row[0].ToString(), Name = row[1].ToString(), Security = row[4].ToString()};
                var price = new Price { Bid = Convert.ToDecimal(row[2].ToString()), Offer = Convert.ToDecimal(row[3].ToString()), UpdatedAt = DateTime.Now };
                repo.SaveOrUpdate<StockPrice>(new StockPrice { Stock = stock, Price = price, Period = period, UpdatedAt = DateTime.Now });
            }
        }

        [Then(@"following stocks price should be updated")]
        public void ThenFollowingStocksPriceShouldBeUpdated(Table table)
        {
            var stocks = new List<Stock>();
            var update = ScenarioContext.Current.Container().Resolve<IUpdate>();
            var repo = ScenarioContext.Current.Container().Resolve<IRepository>();

            foreach (var row in table.Rows)
            {
                stocks.Add(new Stock { Identifier = row[0].ToString(), Name = row[1].ToString() });
            }

            update.Update(stocks);

            foreach (var row in table.Rows)
            {
                Assert.IsFalse(repo.GetAll<StockPrice>().Any(x => x.Stock.Identifier == row[0].ToString()
                                                                && x.Price.Bid == Convert.ToDecimal(row[2].ToString())
                                                                && x.Price.Offer == Convert.ToDecimal(row[3].ToString())));
            }
        }
    }
}
