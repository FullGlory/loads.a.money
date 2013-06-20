namespace SpreadBet.CommandBus
{
    /// <summary>
    /// Command handler interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ICommandHandler<T> where T  : ICommand
    {
        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="command"></param>
        void Execute(T command);
    }
}
