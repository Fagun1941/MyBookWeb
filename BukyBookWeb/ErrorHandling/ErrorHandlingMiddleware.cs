using Microsoft.AspNetCore.Mvc.ViewFeatures;

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
            _logger.LogError(ex, "Unhandled exception occurred.");

            // Save error in TempData
            var factory = context.RequestServices.GetRequiredService<ITempDataDictionaryFactory>();
            var TempData = factory.GetTempData(context);
            TempData["ErrorMessage"] = ex.Message;

            // Redirect to error page
            context.Response.Redirect("/Home/Error");
        }
    }
}
