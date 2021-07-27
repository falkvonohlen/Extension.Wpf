using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace Extension.Wpf.UserControls.Logging
{
    public class LogEvent
    {
        public MappedLogLevel Level { get; set; }

        public string Message { get; set; }

        public string StackTrace { get; set; }

        public LogEvent(LogLevel level, string msg, string stackTrace)
        {
            Level = level switch
            {
                LogLevel.Trace => MappedLogLevel.Trace,
                LogLevel.Debug => MappedLogLevel.Debug,
                LogLevel.Information => MappedLogLevel.Information,
                LogLevel.Warning => MappedLogLevel.Warning,
                LogLevel.Error => MappedLogLevel.Error,
                LogLevel.Critical => MappedLogLevel.Critical,
                _ => MappedLogLevel.None,
            };
            Message = msg;
            StackTrace = stackTrace;
        }
    }

    public enum MappedLogLevel
    {
        Trace = 0,
        Debug = 1,
        Information = 2,
        Warning = 3,
        Error = 4,
        Critical = 5,
        None = 6
    }
}
