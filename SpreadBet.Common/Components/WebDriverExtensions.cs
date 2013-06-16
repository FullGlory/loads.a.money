using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using OpenQA.Selenium;

namespace SpreadBet.Common.Components
{
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
    }
}
