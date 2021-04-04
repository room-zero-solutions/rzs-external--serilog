using Serilog.Events;
using System.Collections.Generic;

namespace RzsSerilog.Api.Logging
{
    public interface IInMemorySink
    {
        IEnumerable<LogEvent> LogEvents { get; }

        void Dispose();

        void Emit(LogEvent logEvent);
    }
}