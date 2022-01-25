﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Extension.Wpf.UserControls.Logging
{
    public static class LogViewBroker
    {
        private static SynchronizationContext _synchronizationContext;

        private static ConcurrentBag<LogView> _openControls = new ConcurrentBag<LogView>();

        public static void RegisterControl(LogView objectRef)
        {
            if (_synchronizationContext == default)
            {
                _synchronizationContext = SynchronizationContext.Current;
            }
            _openControls.Add(objectRef);
        }

        public static void AddLogEvent(string target, LogEvent log)
        {
            _synchronizationContext.Post(new SendOrPostCallback((o) =>
            {
                var control = _openControls.FirstOrDefault(c => c.Id == target);
                if (control != default)
                {
                    control.AddLogEvent(log);
                }
            }), null);
        }
    }
}
