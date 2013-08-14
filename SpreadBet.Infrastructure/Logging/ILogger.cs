namespace SpreadBet.Infrastructure.Logging
{
    using System;

    public interface ILogger
    {
        void Trace(string message);

        void Debug(string message);

        void Info(string message);

        void Warn(string message);

        void Error(Exception ex);

        void Fatal(Exception ex);
    }
}
