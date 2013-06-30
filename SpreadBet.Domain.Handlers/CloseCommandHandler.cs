namespace SpreadBet.Domain.Handlers
{
    using CuttingEdge.Conditions;
    using SpreadBet.Common.Interfaces;
    using SpreadBet.Domain.Commands;
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure.Messaging.Handlers;
    using SpreadBet.Repository;

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
            var result = this._betController.Close(command.Bet);

            if (result)
            {
                _portfolioDataProvider.SaveBet(command.Bet);
            }
        }

        public void Handle(ICommand command)
        {
            Handle(command as CloseBetCommand);
        }
    }
}
