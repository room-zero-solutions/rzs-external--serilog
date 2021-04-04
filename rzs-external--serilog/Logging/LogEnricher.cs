using Microsoft.AspNetCore.Http;
using RzsExternalSerilog.Logging.Extensions;
using Serilog;
using System.Diagnostics;
using System.Linq;

namespace RzsExternalSerilog.Logging
{
    public static class LogEnricher
    {
        public static void EnrichFromRequest(IDiagnosticContext diagnosticContext, HttpContext httpContext)
        {
            Debug.Assert(diagnosticContext != null);
            Debug.Assert(httpContext != null);

            diagnosticContext.Set("ClientIp", $"{httpContext.Connection.RemoteIpAddress}");
            diagnosticContext.Set("UserAgent", httpContext.Request.Headers["User-Agent"].FirstOrDefault());
            diagnosticContext.Set("Resource", httpContext.GetMetricsCurrentResourceName());
        }
    }
}