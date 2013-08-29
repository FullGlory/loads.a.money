using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.Unity.InterceptionExtension;
using SpreadBet.Infrastructure.Logging;

namespace SpreadBet.Infrastructure.Unity.Interception
{
    /// <summary>
    /// Represents a custom interception behaviour that outputs trace statements when injected interfaces are invoked
    /// </summary>
    public class LoggingBehaviour : IInterceptionBehavior
    {
        private readonly ILogger _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingBehaviour"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public LoggingBehaviour(ILogger logger)
        {
            this._logger = logger;
        }
        /// <summary>
        /// Returns the interfaces required by the behavior for the objects it intercepts.
        /// </summary>
        /// <returns>
        /// The required interfaces.
        /// </returns>
        public IEnumerable<Type> GetRequiredInterfaces()
        {
            return Type.EmptyTypes;
        }

        /// <summary>
        /// Implement this method to execute your behavior processing.
        /// </summary>
        /// <param name="input">Inputs to the current call to the target.</param>
        /// <param name="getNext">Delegate to execute to get the next delegate in the behavior chain.</param>
        /// <returns>
        /// Return value from the target.
        /// </returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextInterceptionBehaviorDelegate getNext)
        {
            var sb = new StringBuilder();
            sb.AppendFormat("{0}.{1}(", input.Target.GetType().Name, input.MethodBase.Name);

            if (input.Arguments.Count > 0)
            {
                for (int i = 0; i < input.Inputs.Count; i++)
                {
                    if (i > 0)
                    {
                        sb.Append(",");
                    }

                    sb.AppendFormat("{0}:{1}", input.Inputs.ParameterName(i), input.Inputs[i].ToString());
                }

            }

            sb.Append(")");

            this._logger.Trace("Before: " + sb.ToString());

            IMethodReturn msg = getNext()(input, getNext);

            if (msg.Exception != null)
            {
                this._logger.Error(msg.Exception);

                RecurseError(msg.Exception);
            }

            this._logger.Trace("After: " + sb.ToString());
            return msg;
        }

        private void RecurseError(Exception exception)
        {
            this._logger.Error(exception);

            if (exception.InnerException != null)
            {
                RecurseError(exception.InnerException);
            }
        }

        /// <summary>
        /// Returns a flag indicating if this behavior will actually do anything when invoked.
        /// </summary>
        /// <remarks>
        /// This is used to optimize interception. If the behaviors won't actually
        /// do anything (for example, PIAB where no policies match) then the interception
        /// mechanism can be skipped completely.
        /// </remarks>
        public bool WillExecute
        {
            get { return true; }
        }   
    }
}
