using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extension.Wpf.UserControls.Logging
{
    public class UserControlLoggerConfiguration
    {
        public int EventId { get; set; }
        public string TargetName { get; set; }
        public LogLevel MinLogLevel { get; set; } = LogLevel.Information;
    }
}
