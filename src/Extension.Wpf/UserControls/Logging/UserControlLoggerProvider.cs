using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Extension.Wpf.UserControls.Logging
{
    public sealed class UserControlLoggerProvider : ILoggerProvider
    {
        private readonly UserControlLoggerConfiguration _config;
        private readonly ConcurrentDictionary<string, UserControlLogger> _loggers =
            new ConcurrentDictionary<string, UserControlLogger>();

        public UserControlLoggerProvider(UserControlLoggerConfiguration config) =>
            _config = config;

        public ILogger CreateLogger(string categoryName) =>
            _loggers.GetOrAdd(categoryName, name => new UserControlLogger(name, _config));

        public void Dispose() => _loggers.Clear();
    }
}
