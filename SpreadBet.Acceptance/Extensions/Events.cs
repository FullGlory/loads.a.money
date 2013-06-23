namespace SpreadBet.Acceptance.Extensions
{
    using TechTalk.SpecFlow;
    using SpreadBet.Common.Helpers;
    using SpreadBet.Application;
    using SpreadBet.Infrastructure.Unity;

    [Binding]
    public static class Events
    {
        [BeforeScenario]
        public static void BeforeScenario()
        {
            ScenarioContext.Current.Container(UnityHelper.GetContainer());
        }

        [BeforeFeature]
        public static void BeforeFeature()
        {
            var dbase = new MigrateDatabase();
            dbase.Start();
        }

        [AfterFeature]
        public static void AfterFeature()
        {
            ScenarioContext.Current.Container().Dispose();
        }
    }
}
