using Logic.Models.GenericResponseModel;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace Presentation.Middlewares
{
    public class GlobalExceptionHandler : IMiddleware
    {
        private readonly ILogger<GlobalExceptionHandler> _globalExceptionLogger;
        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> globalExceptionLogger)
        {
            _globalExceptionLogger = globalExceptionLogger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            GenericResponse<bool> res = new GenericResponse<bool>();
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                _globalExceptionLogger.LogError(ex, ex.Message);
                res.InternalError();

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var json = JsonSerializer.Serialize(res);
                context.Response.ContentType = "application/json";

                await context.Response.WriteAsync(json);
            }
        }
    }
}
