using Microsoft.Extensions.Logging;

namespace Zzz.Core.Servers
{
    internal sealed partial class ZzzTrace : ILogger
    {
        private readonly ILogger _generalLogger;

        public ZzzTrace(ILoggerFactory loggerFactory)
        {
            _generalLogger = loggerFactory.CreateLogger("Zzz.Server");
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            => _generalLogger.Log(logLevel, eventId, state, exception, formatter);

        public bool IsEnabled(LogLevel logLevel) => _generalLogger.IsEnabled(logLevel);

        public IDisposable? BeginScope<TState>(TState state) where TState : notnull => _generalLogger.BeginScope(state);

        public void HeartbeatSlow(TimeSpan heartbeatDuration, TimeSpan interval, DateTimeOffset now)
        {
            // while the heartbeat does loop over connections, this log is usually an indicator of threadpool starvation
            GeneralLog.HeartbeatSlow(_generalLogger, now, heartbeatDuration, interval);
        }

        public void ApplicationNeverCompleted(string connectionId)
        {
            GeneralLog.ApplicationNeverCompleted(_generalLogger, connectionId);
        }

        private static partial class GeneralLog
        {
            [LoggerMessage(22, LogLevel.Warning, @"As of ""{now}"", the heartbeat has been running for ""{heartbeatDuration}"" which is longer than ""{interval}"". This could be caused by thread pool starvation.", EventName = "HeartbeatSlow")]
            public static partial void HeartbeatSlow(ILogger logger, DateTimeOffset now, TimeSpan heartbeatDuration, TimeSpan interval);


            [LoggerMessage(23, LogLevel.Critical, @"Connection id ""{ConnectionId}"" application never completed.", EventName = "ApplicationNeverCompleted")]
            public static partial void ApplicationNeverCompleted(ILogger logger, string connectionId);

        }

        #region Connection
        public void ConnectionStart(string connectionId)
        {
            ConnectionsLog.ConnectionStart(_generalLogger, connectionId);
        }

        private static partial class ConnectionsLog
        {
            [LoggerMessage(1, LogLevel.Debug, @"Connection id ""{ConnectionId}"" started.", EventName = "ConnectionStart")]
            public static partial void ConnectionStart(ILogger logger, string connectionId);
        } 
        #endregion
    }
}