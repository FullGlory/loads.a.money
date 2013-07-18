namespace SpreadBet.Common.Components.Automation
{
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain;
    using OpenQA.Selenium.PhantomJS;
    using OpenQA.Selenium;
    using System.Text;
    using System;
    using System.Collections.Generic;
    using SpreadBet.Common.Exceptions;
    using SpreadBet.Common.Entities;
        using System.Linq;

    public class BullBearingsController : IBetController
    {
        private readonly IAutomationProvider _provider;

        public BullBearingsController(IAutomationProvider provider)
        {
            _provider = provider;
        }

        public bool Open(Bet bet)
        {
            try
            {
                if (_provider.Authenticate(bet.Account))
                    return _provider.Open(bet);
                return false;
            }
            catch (AutomationException ex)
            {
                Console.WriteLine("Error " + ex.Error + " : " + ex.Message);
                return false;
            }           
        }

        public bool Close(Bet bet)
        {
            try
            {
                if (_provider.Authenticate(bet.Account))
                    return _provider.Close(bet);
                return false;
            }
            catch (AutomationException ex)
            {
                Console.WriteLine("Error " + ex.Error + " : " + ex.Message);
                return false;
            } 
        }

    }
}
