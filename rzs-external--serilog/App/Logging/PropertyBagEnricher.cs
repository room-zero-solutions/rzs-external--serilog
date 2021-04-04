using Serilog.Core;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace RzsSerilog.App.Logging
{
    public class PropertyBagEnricher : ILogEventEnricher
    {
        private readonly Dictionary<string, Tuple<object, bool>> _properties;

        public PropertyBagEnricher()
        {
            _properties = new Dictionary<string, Tuple<object, bool>>(StringComparer.OrdinalIgnoreCase);

            ////    _logger
            ////.ForContext(
            ////  new PropertyBagEnricher()
            ////    .Add("ResponseCode", response?.ResponseCode)
            ////    .Add("EnrollmentStatus", response?.Enrolled)
            ////)
            ////.Warning("Malfunction when processing 3DS enrollment verification");
        }

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Debug.Assert(logEvent != null);
            Debug.Assert(propertyFactory != null);

            foreach (var prop in _properties)
            {
                logEvent.AddPropertyIfAbsent(propertyFactory.CreateProperty(prop.Key, prop.Value.Item1, prop.Value.Item2));
            }
        }

        public PropertyBagEnricher Add(string key, object value, bool destructureObject = false)
        {
            if (string.IsNullOrEmpty(key)) throw new ArgumentNullException(nameof(key));

            if (!_properties.ContainsKey(key)) _properties.Add(key, Tuple.Create(value, destructureObject));

            return this;
        }
    }
}