using System;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.Extensions.Logging;

namespace TwilightSparkle.Forum.Middlewares
{
    public class ErrorLoggerMiddleware
    {
        private readonly RequestDelegate _next;


        public ErrorLoggerMiddleware(RequestDelegate next)
        {
            _next = next;
        }


        public async Task InvokeAsync(HttpContext context, ILogger<ErrorLoggerMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Exception while sending '{context.Request.Method} {context.Request.GetDisplayUrl()}'");

                throw;
            }
        }
    }
}
