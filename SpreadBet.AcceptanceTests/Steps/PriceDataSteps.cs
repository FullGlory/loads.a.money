using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using SpreadBet.AcceptanceTests.Agents;
using TechTalk.SpecFlow;

namespace SpreadBet.AcceptanceTests.Steps
{
    [Binding]
    public class PriceDataSteps
    {
        [When(@"I check the content of the stock price database")]
        public void WhenICheckTheContentOfTheStockPriceDatabase()
        {
           // Do thing, the next step will do this
        }

        [Then(@"stock price data appears within (.*) minutes")]
        public void ThenStockPriceDataAppearsWithinMinutes(int amount)
        {
            var agent = ScenarioContext.Current.Get<IPriceDataAgent>();

            var count = agent.GetStockPriceCount();

            // TODO - parameterise the units (mins, secs etc)
            var periodToWait = TimeSpan.FromMinutes(amount);

            var task = new AgentTask(() => { return agent.GetStockPriceCount() > count; }, periodToWait);

            Assert.IsTrue(task.WaitUntilDone());
        }
    }
}
