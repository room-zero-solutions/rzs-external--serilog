using RzsExternalSerilog.Api.Logging;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Threading;

namespace RzsExternalSerilog.Logging
{
    public class InMemorySink : ILogEventSink, IDisposable, IInMemorySink
    {
        private static readonly AsyncLocal<InMemorySink> _localInstance = new();

        private static readonly List<LogEvent> _logEvents = new();

        public static InMemorySink Instance
        {
            get
            {
                if (_localInstance.Value == null)
                    _localInstance.Value = new InMemorySink();

                return _localInstance.Value;
            }
        }

        public IEnumerable<LogEvent> LogEvents => _logEvents.AsReadOnly();

        public void Emit(LogEvent logEvent) => _logEvents.Add(logEvent);

        protected virtual void Dispose(bool disposed)
        {
            _logEvents.Clear();
        }

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }
    }
}