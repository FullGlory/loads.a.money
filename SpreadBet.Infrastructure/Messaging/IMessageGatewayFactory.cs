using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadBet.Infrastructure.Messaging
{
    public interface IMessageGatewayFactory
    {
        IMessageSender GetSenderInstance(string channelName);
    }
}
