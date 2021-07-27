using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Extension.Wpf.UserControls.Logging
{
    public static class LogViewBroker
    {
        private static ConcurrentBag<LogView> _openControls = new ConcurrentBag<LogView>();

        public static void RegisterControl(LogView objectRef)
        {
            _openControls.Add(objectRef);
        }

        public static void AddLogEvent(string target, LogEvent log)
        {
            var control = _openControls.FirstOrDefault(c => c.Id == target);
            if (control != default)
            {
                control.AddLogEvent(log);
            }
        }
    }
}
