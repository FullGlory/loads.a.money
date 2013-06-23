using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadBet.Infrastructure.Messaging
{
    /// <summary>
    /// Static factory class for Envelope
    /// </summary>
    public abstract class Envelope
    {
        public static Envelope<T> Create<T>(T body)
        {
            return new Envelope<T>(body);
        }
    }

    public class Envelope<T> : Envelope
    {
        public Envelope(T body)
        {
            this.Body = body;
        }

        public T Body { get; private set; }

        public static implicit operator Envelope<T>(T body) 
        { 
            return Envelope.Create(body); 
        }
    }
}
