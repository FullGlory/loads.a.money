namespace SpreadBet.Infrastructure.Messaging.Handlers
{
    using SpreadBet.Infrastructure.Serialisation;

    public class CommandProcessor : MessageProcessor
    {
        private readonly ICommandHandlerRegistry _registry;

        public CommandProcessor(IMessageReceiver receiver, ITextSerialiser serialiser, ICommandHandlerRegistry registry)
            :base(receiver, serialiser)
        {
            this._registry = registry;
        }

        protected override void ProcessMessage(object payload)
        {
            var command = payload as ICommand;

            if (command != null)
            {
                var handler = this._registry.GetCommandHandler(command);

                if (handler != null)
                {
                    handler.Handle(command);
                }
            }
        }
    }
}
