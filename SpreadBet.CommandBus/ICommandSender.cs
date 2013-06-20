namespace SpreadBet.CommandBus
{
    /// <summary>
    /// A contract allowing commands to be sent to the domain
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends the given command to the domain
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="command"></param>
        void Send<T>(T command) where T : ICommand;
    }
}
