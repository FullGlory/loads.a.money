namespace SpreadBet.Domain.Handlers
{
    using CuttingEdge.Conditions;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure.Messaging.Handlers;

    public class CloseCommandHandler : ICommandHandler<CloseBetCommand>
    {
        private readonly IPortfolioDataProvider _portfolioDataProvider;
        private readonly IBetController _betController;

        public CloseCommandHandler(
            IPortfolioDataProvider portfolioDataProvider, 
            IBetController betController)
        {
            Condition.Requires(portfolioDataProvider).IsNotNull();
            Condition.Requires(betController).IsNotNull();

            this._portfolioDataProvider = portfolioDataProvider;
            this._betController = betController;
        }

        public void Handle(CloseBetCommand command)
        {
            var bet = this._portfolioDataProvider.Get(command.BetId);

            var result = this._betController.Close(bet);

            if (result)
            {
                _portfolioDataProvider.SaveBet(bet);
            }
        }

        public void Handle(ICommand command)
        {
            Handle(command as CloseBetCommand);
        }
    }
}
