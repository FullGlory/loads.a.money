namespace SpreadBet.Common.Components.Automation
{
    using System;
    using SpreadBet.Common.Interfaces;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Remote;

    public class PhantomDriver : IAutomationDriver, IDisposable
    {
        private PhantomJSDriver _driver;

        public RemoteWebDriver WebDriver
        {
            get
            {
                if (_driver == null)
                {
                    var configOptions = new PhantomJSOptions();
                    configOptions.AddAdditionalCapability("--webdriver", 9999);
                    configOptions.AddAdditionalCapability("--load-images", false);

                    _driver = new PhantomJSDriver(configOptions);
                }
                return _driver;
            }
        }

        public void Dispose() 
        {
            Dispose(true);
            GC.SuppressFinalize(this); 
        }

        protected virtual void Dispose(bool disposing) 
        {
            if (disposing) 
            {
                //TODO - dispose "may" quit automatically
                WebDriver.Quit();
                WebDriver.Dispose();
            }
        }

        // Use C# destructor syntax for finalization code.
        ~PhantomDriver()
        {
            // Simply call Dispose(false).
            Dispose (false);
        }

    }
}
