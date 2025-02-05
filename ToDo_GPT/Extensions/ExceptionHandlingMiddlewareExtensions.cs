using Microsoft.AspNetCore.Diagnostics;
using ToDo_GPT.Logging;
using System.Net;
using ToDo_GPT.Models;

namespace ToDo_GPT.Extensions;

public static class ExceptionHandlingMiddlewareExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app, CustomerLogger logger)
    {
        app.UseExceptionHandler(appError =>
        {
            appError.Run(async context =>
            {
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                if (contextFeature != null)
                {
                    logger.LogError($"Something went wrong: SatatusCode: {context.Response.StatusCode}, " +
                        $"Message: {contextFeature.Error.Message}");

                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace
                    }.ToString());
                }
            });
        });
    }
}