using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;
using System;
using System.Diagnostics;
using System.Linq;

namespace RzsSerilog.App.Logging.Extensions
{
    public static class SerilogExtensions
    {
        private const string DefaultOutputTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}";

        public static string GetCorrelationId(this HttpContext httpContext)
        {
            Debug.Assert(httpContext != null);

            httpContext.Request.Headers.TryGetValue("Cko-Correlation-Id", out var correlationId);

            return correlationId.FirstOrDefault() ?? httpContext.TraceIdentifier;
        }

        public static string GetMetricsCurrentResourceName(this HttpContext httpContext)
        {
            Debug.Assert(httpContext != null);

            return $"{httpContext.Request.Path}";
        }

        public static LoggerConfiguration WithBuildInfo(this LoggerEnrichmentConfiguration config)
        {
            Debug.Assert(config != null);

            return config.With<BuildInfoEnricher>();
        }

        public static LoggerConfiguration InMemory(
            this LoggerSinkConfiguration configuration,
            LogEventLevel restrictedToMinimumLevel = LevelAlias.Minimum,
            string outputTemplate = DefaultOutputTemplate,
            LoggingLevelSwitch? levelSwitch = null)
        {
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));
            if (outputTemplate == null) throw new ArgumentNullException(nameof(outputTemplate));

            return configuration.Sink(InMemorySink.Instance, restrictedToMinimumLevel, levelSwitch);
        }

        public static IApplicationBuilder UseRzsSerilog(this IApplicationBuilder app)
        {
            app.UseMiddleware<RequestLogContextMiddleware>();

            app.UseSerilogRequestLogging(options => options.EnrichDiagnosticContext = LogEnricher.EnrichFromRequest);

            return app;
        }
    }
}