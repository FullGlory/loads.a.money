namespace SpreadBet.Infrastructure.Messaging.Handlers
{
    public interface ICommandHandler
    {
        void Handle(ICommand command);
    }

    /// <summary>
    /// Command handler interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandHandler<T> : ICommandHandler where T : ICommand
    {
        /// <summary>
        /// Handles the command
        /// </summary>
        /// <param name="command"></param>
        void Handle(T command);
    }
}
