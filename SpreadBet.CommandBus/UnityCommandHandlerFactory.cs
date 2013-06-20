using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SpreadBet.Common.Unity;
using Microsoft.Practices.Unity;

namespace SpreadBet.CommandBus
{
    public class UnityCommandHandlerFactory : ICommandHandlerFactory
    {
        public ICommandHandler<T> CreateCommandHandler<T>(T Command) where T : ICommand
        {
            var container = UnityHelper.GetContainer();

            return container.Resolve<ICommandHandler<T>>();
        }
    }
}
