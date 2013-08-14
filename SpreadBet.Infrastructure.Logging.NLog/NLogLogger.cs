using System;
using NLog;

namespace SpreadBet.Infrastructure.Logging.NLog
{
    public class NLogLogger : ILogger
    {
        private Logger _logger;

        public NLogLogger()
        {
            this._logger = LogManager.GetCurrentClassLogger();
        }

        public void Trace(string message)
        {
            this._logger.Trace(message);
        }

        public void Debug(string message)
        {
            this._logger.Debug(message);
        }

        public void Info(string message)
        {
            this._logger.Info(message);
        }

        public void Warn(string message)
        {
            this._logger.Warn(message);
        }

        public void Error(Exception ex)
        {
            this._logger.ErrorException(ex.Message, ex);
        }

        public void Fatal(Exception ex)
        {
            this._logger.FatalException(ex.Message, ex);
        }
    }
}
