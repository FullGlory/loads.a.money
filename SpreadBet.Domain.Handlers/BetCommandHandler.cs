namespace SpreadBet.Domain.Handlers
{
    using CuttingEdge.Conditions;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure.Messaging.Handlers;
    
    public class BetCommandHandler : ICommandHandler<PlaceBetCommand>
    {
        private readonly IPortfolioDataProvider _portfolioDataProvider;
        private readonly IAccountDataProvider _accountDataProvider;
        private readonly IStockDataProvider _stockDataProvider;
        private readonly IBetController _betController;

        public BetCommandHandler(
            IPortfolioDataProvider portfolioDataProvider, 
            IAccountDataProvider accountDataProvider,
            IStockDataProvider stockDataProvider, 
            IBetController betController)
        {
            Condition.Requires(portfolioDataProvider).IsNotNull();
            Condition.Requires(accountDataProvider).IsNotNull();
            Condition.Requires(stockDataProvider).IsNotNull();
            Condition.Requires(betController).IsNotNull();

            this._accountDataProvider = accountDataProvider;
            this._portfolioDataProvider = portfolioDataProvider;
            this._stockDataProvider = stockDataProvider;
            this._betController = betController;
        }

        public void Handle(PlaceBetCommand command)
        {
            var stock = this._stockDataProvider.GetStock(command.StockIdentifier);

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
                this._portfolioDataProvider.SaveBet(bet);
            }
        }

        public void Handle(ICommand command)
        {
            Handle(command as PlaceBetCommand);
        }
    }
}
