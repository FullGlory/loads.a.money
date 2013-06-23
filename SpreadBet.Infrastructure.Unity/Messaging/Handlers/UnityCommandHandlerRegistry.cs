namespace SpreadBet.Infrastructure.Unity.Messaging.Handlers
{
    using Microsoft.Practices.Unity;
    using SpreadBet.Infrastructure.Messaging;
    using SpreadBet.Infrastructure.Messaging.Handlers;

    public class UnityCommandHandlerRegistry : ICommandHandlerRegistry
    {
        public ICommandHandler GetCommandHandler<T>(T Command) where T : ICommand
        {
            // TODO - ideally, the return type needs to be ICommandHandler<T>
            var handlerType = typeof(ICommandHandler<>).MakeGenericType(Command.GetType());
            var handler = UnityHelper.GetContainer().Resolve(handlerType);

            return (ICommandHandler)handler;
        }
    }
}
