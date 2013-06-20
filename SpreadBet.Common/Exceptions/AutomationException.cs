using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadBet.Common.Exceptions
{
    public class AutomationException : System.Exception
    {
        public string Error { get; set; }

        public AutomationException(string error, string message)
            : base(message)
        {
            Error = error;
        }
    }
}
