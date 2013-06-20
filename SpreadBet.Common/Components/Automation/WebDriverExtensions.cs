namespace SpreadBet.Common.Components.Automation
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using OpenQA.Selenium;

    public static class WebDriverExtensions
    {
        public static void Wait(this IWebDriver driver, Func<bool> isDone, int timeOut = 5000)
        {
            var stopWatch = Stopwatch.StartNew();

            while (stopWatch.ElapsedMilliseconds < timeOut && !isDone())
            {
                System.Threading.Thread.Sleep(500);
            }
        }

        public static bool VerifyRoute(this IWebDriver driver, string baseUrl, string page, string route, string param = null)
        {
            var routing = BuildRoute(baseUrl, page, route, param);
            driver.Navigate().GoToUrl(routing);
            return driver.Url.Equals(routing);
        }

        public static void Route(this IWebDriver driver, string baseUrl, string page, string route, string param = null)
        {
            var routing = BuildRoute(baseUrl, page, route, param);
            driver.Navigate().GoToUrl(routing);
        }

        #region Helpers

        private static string BuildRoute(string baseUrl, string page, string route, string param = null)
        {
            var url = baseUrl + route + page;
            return (param != null) ? url + "?" + param : url;
        }

        #endregion
    }
}
