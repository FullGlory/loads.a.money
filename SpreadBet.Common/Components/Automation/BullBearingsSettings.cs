namespace SpreadBet.Common.Components.Automation
{
    using SpreadBet.Common.Interfaces;
    using OpenQA.Selenium;
    using OpenQA.Selenium.PhantomJS;

    public class BullBearingsSettings : IAutomationSettings
    {
        private static string __BASEURL = "http://www.bullbearings.co.uk/";
        private static string __PAGE = ".php";

        public string BaseUrl
        {
            get
            {
                return __BASEURL;
            }
        }

        public string Page
        {
            get
            {
                return __PAGE;
            }
        }
    }
}
