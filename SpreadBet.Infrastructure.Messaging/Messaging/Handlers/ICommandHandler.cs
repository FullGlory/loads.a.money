namespace SpreadBet.Infrastructure.Messaging.Handlers
{
    /// <summary>
    /// Command handler interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandHandler<T> where T : ICommand
    {
        /// <summary>
        /// Handles the command
        /// </summary>
        /// <param name="command"></param>
        void Handle(T command);
    }
}
