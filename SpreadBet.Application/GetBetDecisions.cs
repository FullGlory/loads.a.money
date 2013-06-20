//// -----------------------------------------------------------------------
//// <copyright file="GetBetDecisions.cs" company="">
//// TODO: Update copyright text.
//// </copyright>
//// -----------------------------------------------------------------------

namespace SpreadBet.Application
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using SpreadBet.Common.Interfaces;
    using CuttingEdge.Conditions;
    using SpreadBet.Domain.Commands;
    using SpreadBet.CommandBus;


    /// <summary>
    /// Returns a list of bet decisions
    /// </summary>
    public class GetBetDecisions : IExecutableApplication
    {
        private IStockDataProvider _stockDataProvider;
        private IStockFilter _stockFilter;
        private IInvestDecider _investDecider;
        private ICommandSender _commandBus;
        private IUpdate _priceUpdate;

        public GetBetDecisions(
            IStockDataProvider stockDataProvider,
            IStockFilter stockFilter,
            IInvestDecider investDecider,
            ICommandSender commandBus,
            IUpdate priceUpdate)
        {
            Condition.Requires(stockDataProvider).IsNotNull();
            Condition.Requires(stockFilter).IsNotNull();
            Condition.Requires(investDecider).IsNotNull();
            Condition.Requires(commandBus).IsNotNull();
            Condition.Requires(priceUpdate).IsNotNull();

            this._stockDataProvider = stockDataProvider;
            this._stockFilter = stockFilter;
            this._investDecider = investDecider;
            this._commandBus = commandBus;
            this._priceUpdate = priceUpdate;

        }
        public void Run()
        {
            // Get all the stocks we know about
            var stocks = this._stockDataProvider.GetStocks();

            // Determine which ones look interesting
            var potentialCandidates = this._stockFilter.GetInvestmentCandidates(stocks);

            // Update final stock price from live data feed#
            this._priceUpdate.Update(potentialCandidates);

            // Rerun process for potential candiadtes only
            var candidates = this._stockFilter.GetInvestmentCandidates(potentialCandidates);

            // Determine what the bets might look like
            var bets = this._investDecider.GetInvestmentDescisions(candidates);

            foreach (var bet in bets)
            {
                // Create a command to represent an update to the domain
                var command = new PlaceBetCommand
                {
                    BidAmount = bet.BidAmount,
                    ExitPrice = bet.ExitPrice,
                    InitialLoss = bet.InitialLoss,
                    IsIncrease = (bet.Direction == Domain.Direction.Increase),
                    OpeningPosition = bet.OpeningPosition,
                    StockIdentifier = bet.Stock.Identifier
                };

                // Send the command for processing
                this._commandBus.Send(command);
            }
        }
    }
}