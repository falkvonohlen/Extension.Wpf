using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extension.Wpf.UserControls.Logging
{
    public class UserControlLogger : ILogger
    {
        private readonly string _name;
        private readonly UserControlLoggerConfiguration _config;

        public UserControlLogger(
            string name,
            UserControlLoggerConfiguration config) =>
            (_name, _config) = (name, config);

        public IDisposable BeginScope<TState>(TState state) => default;

        public bool IsEnabled(LogLevel logLevel) =>
            logLevel >= _config.MinLogLevel;

        public void Log<TState>(
            LogLevel logLevel,
            EventId eventId,
            TState state,
            Exception exception,
            Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            LogViewBroker.AddLogEvent(_config.TargetName, new LogEvent(logLevel, formatter(state, exception), exception?.StackTrace));
        }
    }
}
