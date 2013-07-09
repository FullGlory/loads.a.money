namespace SpreadBet.Domain.Commands
{
    using SpreadBet.Infrastructure.Messaging;

    public class CloseBetCommand : ICommand
    {
        public int BetId { get; set; }
    }
}
