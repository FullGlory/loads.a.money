using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.AcceptanceTests.Agents;
using TechTalk.SpecFlow;

namespace SpreadBet.AcceptanceTests.Steps
{
    [Binding]
    public class Hooks
    {
        // For additional details on SpecFlow hooks see http://go.specflow.org/doc-hooks

        [BeforeScenario]
        public void BeforeScenario()
        {
            var agent = new SpreadBetAgent();
            ScenarioContext.Current.Set<IPriceDataAgent>(agent);
        }

        [AfterScenario]
        public void AfterScenario()
        {
            //TODO: implement logic that has to run after executing each scenario
        }
    }
}
