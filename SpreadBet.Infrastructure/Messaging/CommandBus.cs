namespace SpreadBet.Infrastructure.Messaging
{
    using System;
    using System.IO;
    using SpreadBet.Infrastructure.Serialisation;

    public class CommandBus : ICommandBus
    {
        private readonly IMessageSender _messageSender;
        private readonly ITextSerialiser _textSerialiser;

        public CommandBus(IMessageSender messageSender, ITextSerialiser textSerialiser)
        {
            this._messageSender = messageSender;
            this._textSerialiser = textSerialiser;
        }

        public void Send(Envelope<ICommand> command)
        {
            var message = BuildMessage(command);

            this._messageSender.Send(message);
        }

        private Message BuildMessage(Envelope<ICommand> command)        
        {            
            using (var payloadWriter = new StringWriter())            
            {
                this._textSerialiser.Serialize(payloadWriter, command.Body);
                return new Message{ Body = payloadWriter.ToString()};
            }        
        }
    }
}
