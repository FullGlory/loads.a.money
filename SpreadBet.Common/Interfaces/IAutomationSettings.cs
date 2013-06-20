using OpenQA.Selenium;
using OpenQA.Selenium.PhantomJS;

namespace SpreadBet.Common.Interfaces
{
    public interface IAutomationSettings
    {
        string BaseUrl { get; }
        string Page { get; }
    }
}
