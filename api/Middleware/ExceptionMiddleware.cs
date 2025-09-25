using Microsoft.AspNetCore.Http;
using System.Net;
using System.Threading.Tasks;
using Stargate.Repositories;
using StargateAPI.Business.Data;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, IProcessLogRepository logRepo)
    {
        try
        {
            await _next(context);
        }
        catch(Exception ex)
        {
            // log to DB
            await logRepo.AddLogAsync(new ProcessLog
            {
                Level = "ERROR",
                Message = ex.Message,
                StackTrace = ex.StackTrace,
                Context = context.Request.Path
            });

            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            await context.Response.WriteAsJsonAsync(new { error = "An unexpected error occurred." });
        }
    }
}