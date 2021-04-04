using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;

namespace RzsSerilog.App.Logging
{
    public class BuildInfoEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            Debug.Assert(logEvent != null);
            Debug.Assert(propertyFactory != null);

            AddProperty(logEvent, propertyFactory, "RZS_BUILD_ID");
            AddProperty(logEvent, propertyFactory, "RZS_GITHUB_BRANCH");
            AddProperty(logEvent, propertyFactory, "RZS_GITHUB_SHA");
            AddProperty(logEvent, propertyFactory, "RZS_TIMESTAMP");
        }

        private static void AddProperty(LogEvent logEvent, ILogEventPropertyFactory propertyFactory, string name)
        {
            var eventId = propertyFactory.CreateProperty(name, Environment.GetEnvironmentVariable(name));

            logEvent.AddPropertyIfAbsent(eventId);
        }
    }
}