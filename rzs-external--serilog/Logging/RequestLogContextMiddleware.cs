using Microsoft.AspNetCore.Http;
using RzsExternalSerilog.Logging.Extensions;
using Serilog.Context;
using System.Threading.Tasks;

namespace RzsExternalSerilog.Logging
{
    public class RequestLogContextMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLogContextMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("CorrelationId", context.GetCorrelationId()))
            {
                return _next.Invoke(context);
            }
        }
    }
}