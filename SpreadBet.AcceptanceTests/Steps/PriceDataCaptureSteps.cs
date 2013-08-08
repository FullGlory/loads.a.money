using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechTalk.SpecFlow;

namespace SpreadBet.AcceptanceTests.Steps
{
    [Binding]
    public class PriceDataCaptureSteps
    {
        [Given(@"the price data capture routine is running")]
        public void GivenThePriceDataCaptureRoutineIsRunning()
        {
            // TODO - routine = windows service, so assume it's running
        }
    }
}
