namespace SpreadBet.Acceptance.Extensions
{
    using System;
    using TechTalk.SpecFlow;
    using Microsoft.Practices.Unity;
    using System.Collections.Generic;
    using SpreadBet.Domain;
    using SpreadBet.Common.Interfaces;

    public static class ScenarioContextExt
    {
        private static string __CONTAINER = "CONTAINER";
        private static string __BETS = "BETS";
        private static string __ACCOUNT = "ACCOUNT";

        public static void Container(this ScenarioContext ctx, IUnityContainer container)
        {
            ctx.Add(__CONTAINER, container);
        }

        public static IUnityContainer Container(this ScenarioContext ctx)
        {
            return ctx.Get<IUnityContainer>(__CONTAINER);
        }

        public static void Bets(this ScenarioContext ctx, List<Bet> bets)
        {
            ctx.Add(__BETS, bets);
        }

        public static List<Bet> Bets(this ScenarioContext ctx)
        {
            return ctx.Get<List<Bet>>(__BETS);
        }

        public static void Account(this ScenarioContext ctx, Account account)
        {
            ctx.Add(__ACCOUNT, account);
        }

        public static Account Account(this ScenarioContext ctx)
        {
            return ctx.Get<Account>(__ACCOUNT);
        }
    }
}
