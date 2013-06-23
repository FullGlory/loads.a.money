using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadBet.Infrastructure.Messaging
{
    public interface ICommandBus
    {
        void Send(Envelope<ICommand> command);
    }
}
