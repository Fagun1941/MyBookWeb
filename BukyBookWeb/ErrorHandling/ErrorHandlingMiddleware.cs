using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Serilog; // for LogContext if you use correlation Guid

public class ErrorHandlingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(RequestDelegate next, ILogger<ErrorHandlingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            var logGuid = context.Items.ContainsKey("LogGuid")
                ? context.Items["LogGuid"]?.ToString()
                : Guid.NewGuid().ToString();

            // Push LogGuid into Serilog so it gets written to MSSqlServer
            using (Serilog.Context.LogContext.PushProperty("LogGuid", logGuid))
            {
                Log.Error(ex, "Unhandled exception occurred. CorrelationId={LogGuid}", logGuid);
            }

            // Save error in TempData
            var factory = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var tempData = factory.GetTempData(context);
            tempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
            tempData["ErrorDetails"] = ex.Message;

            var provider = context.RequestServices.GetRequiredService<ITempDataProvider>();
            provider.SaveTempData(context, tempData);

            context.Response.Clear();
            context.Response.Redirect("/Home/Error");
        }

    }
}
