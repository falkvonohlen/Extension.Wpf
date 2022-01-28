using System;
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

        private static ConcurrentDictionary<string, LogView> _openControls = new ConcurrentDictionary<string, LogView>();

        private static ConcurrentDictionary<string, ConcurrentQueue<LogEvent>> _buffer = new ConcurrentDictionary<string, ConcurrentQueue<LogEvent>>();

        public static void RegisterControl(string id, LogView view)
        {
            if (_synchronizationContext == default)
            {
                _synchronizationContext = SynchronizationContext.Current;
            }
            _openControls.AddOrUpdate(id, view, (key, old) => view);
            if (_buffer.TryGetValue(id, out var buffered))
            {
                while (buffered.TryDequeue(out var log))
                {
                    _synchronizationContext.Post(new SendOrPostCallback((o) => view.AddLogEvent(log)), null);
                }
            }
        }

        public static void UnregisterControl(string id)
        {
            if (_synchronizationContext == default)
            {
                _synchronizationContext = SynchronizationContext.Current;
            }
            _openControls.TryRemove(id, out var _);
        }

        public static void AddLogEvent(string target, LogEvent log)
        {
            if (_openControls.TryGetValue(target, out var view))
            {
                _synchronizationContext.Post(new SendOrPostCallback((o) => view.AddLogEvent(log)), null);
            }
            else
            {
                _buffer.AddOrUpdate(target, target =>
                {
                    var queue = new ConcurrentQueue<LogEvent>();
                    queue.Enqueue(log);
                    return queue;
                },
                (target, old) =>
                {
                    old.Enqueue(log);
                    return old;
                });
            }
        }
    }
}
