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
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure;

    /// <summary>
    /// Returns a list of bet decisions
    /// </summary>
    public class CloseBetDecisions : IProcessor
    {
        private IPortfolioDataProvider _portfolioDataProvider;
        private IExitDecider _exitDecider;
        private ICommandBus _commandBus;
        private IUpdate _priceUpdate;

        public CloseBetDecisions(
            IPortfolioDataProvider portfolioDataProvider,
            IExitDecider exitDecider,
            ICommandBus commandBus,
            IUpdate priceUpdate)
        {
            Condition.Requires(portfolioDataProvider).IsNotNull();
            Condition.Requires(exitDecider).IsNotNull();
            Condition.Requires(commandBus).IsNotNull();
            Condition.Requires(priceUpdate).IsNotNull();

            this._portfolioDataProvider = portfolioDataProvider;
            this._exitDecider = exitDecider;
            this._commandBus = commandBus;
            this._priceUpdate = priceUpdate;

        }

        public void Start()
        {
            // Get all the active bets
            var bets = this._portfolioDataProvider.GetCurrentBets();

            // Determine which ones need to be closed
            var potentialCandidates = this._exitDecider.GetExitDescisions(bets);

            // Update final stock price for bet from live data feed#
            this._priceUpdate.Update(potentialCandidates);

            // Rerun process for potential candiadtes only
            var candidates = this._exitDecider.GetExitDescisions(potentialCandidates);

            foreach (var bet in candidates)
            {
                // Create a command to represent an update to the domain
                var command = new CloseBetCommand
                {
                    Bet = bet,
                };

                // Send the command for processing
                this._commandBus.Send(command);
            }
        }

        public void Stop()
        {
            // Do nothing
        }
    }
}