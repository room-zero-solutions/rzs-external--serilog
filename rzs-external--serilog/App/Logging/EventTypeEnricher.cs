using Murmur;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Text;

namespace RzsSerilog.App.Logging
{
    public class EventTypeEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            if (logEvent is null) throw new ArgumentNullException(nameof(logEvent));

            if (propertyFactory is null) throw new ArgumentNullException(nameof(propertyFactory));

            using var murmur = MurmurHash.Create32();

            var bytes = Encoding.UTF8.GetBytes(logEvent.MessageTemplate.Text);

            var hash = murmur.ComputeHash(bytes);

            var hexadecimalHash = BitConverter.ToString(hash).Replace("-", "", StringComparison.OrdinalIgnoreCase);

            var eventId = propertyFactory.CreateProperty("EventType", hexadecimalHash);

            logEvent.AddPropertyIfAbsent(eventId);
        }
    }
}