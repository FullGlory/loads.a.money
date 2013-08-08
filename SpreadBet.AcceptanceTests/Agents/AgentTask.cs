using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SpreadBet.AcceptanceTests.Agents
{
    public class AgentTask
    {
        public static Action<string> Log = (message) => { Console.WriteLine(message); };

        public AgentTask(Func<bool> operation, TimeSpan timeout)
        {
            this.Operation = operation;
            this.Timeout = timeout;

            // TODO - make this static or defined per task?
            this.OperationIntervalTimeout = TimeSpan.FromMilliseconds(500);
        }

        public Func<bool> Operation { get; private set; }
        public TimeSpan Timeout { get; private set; }
        public TimeSpan OperationIntervalTimeout { get; set; }

        public bool WaitUntilDone()
        {
            var taskSignaled = false;

            var timer = Stopwatch.StartNew();

            while (!taskSignaled && timer.Elapsed < this.Timeout)
            {
                taskSignaled = this.Operation();

                if (!taskSignaled)
                {
                    Log(string.Format("Sleeping for {0}ms. Time left is {1:0}s", this.OperationIntervalTimeout.TotalMilliseconds, (this.Timeout - timer.Elapsed).TotalSeconds));

                    Thread.Sleep(this.OperationIntervalTimeout);
                }
            }

            return taskSignaled;
        }
    }
}
