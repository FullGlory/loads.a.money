namespace SpreadBet.CommandBus
{
    /// <summary>
    /// Factory interface for command handlers
    /// </summary>
    public interface ICommandHandlerFactory
    {
        ICommandHandler<T> CreateCommandHandler<T>(T Command) where T : ICommand;
    }
}
