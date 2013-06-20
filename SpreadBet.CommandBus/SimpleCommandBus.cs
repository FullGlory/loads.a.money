namespace SpreadBet.CommandBus
{
    using CuttingEdge.Conditions;

    public class SimpleCommandBus : ICommandSender
    {
        private ICommandHandlerFactory _commandHandlerFactory;

        public SimpleCommandBus(ICommandHandlerFactory commandHandlerFactory)
        {
            Condition.Requires(commandHandlerFactory).IsNotNull();
            this._commandHandlerFactory = commandHandlerFactory;
        }

        public void Send<T>(T command) where T : ICommand
        {
            var handler = this._commandHandlerFactory.CreateCommandHandler(command);

            handler.Execute(command);
        }
    }
}
