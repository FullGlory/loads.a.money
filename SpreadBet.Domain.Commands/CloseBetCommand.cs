namespace SpreadBet.Domain.Commands
{
    using SpreadBet.Infrastructure.Messaging;

    public class CloseBetCommand : ICommand
    {
        public Bet Bet { get; set; }
    }
}
