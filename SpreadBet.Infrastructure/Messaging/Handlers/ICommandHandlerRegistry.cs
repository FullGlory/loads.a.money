namespace SpreadBet.Infrastructure.Messaging.Handlers
{
    public interface ICommandHandlerRegistry
    {
        ICommandHandler GetCommandHandler<T>(T Command) where T : ICommand;
    }
}
