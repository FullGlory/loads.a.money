namespace SpreadBet.Domain.CommandHandlers
{
    using System;
    using CuttingEdge.Conditions;
    using SpreadBet.CommandBus;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Repository;

    public class PlaceBetCommandHandler : ICommandHandler<PlaceBetCommand>
    {
        private readonly IAccountDataProvider _accountDataProvider;
        private readonly IRepository _repository;
        private readonly IBetController _betController;

        public PlaceBetCommandHandler(
            IAccountDataProvider accountDataProvider, 
            IRepository repository, 
            IBetController betController)
        {
            Condition.Requires(accountDataProvider).IsNotNull();
            Condition.Requires(repository).IsNotNull();
            Condition.Requires(betController).IsNotNull();

            this._accountDataProvider = accountDataProvider;
            this._repository = repository;
            this._betController = betController;
        }
        public void Execute(PlaceBetCommand command)
        {
            var stock = this._repository.Get<Stock>(s => s.Identifier.Equals(command.StockIdentifier));

            var bet = new Bet
            {
                BidAmount = command.BidAmount,
                Direction = command.IsIncrease ? Direction.Increase : Direction.Decrease,
                ExitPrice = command.ExitPrice,
                InitialLoss = command.InitialLoss,
                OpeningPosition = command.OpeningPosition,
                Stock = stock,
                Account = this._accountDataProvider.GetCurrentPosition()
            };

            var result = this._betController.Open(bet);

            if (result)
            {
                // TODO - save bet, raise an event to do this later.....?
            }
        }
    }
}
