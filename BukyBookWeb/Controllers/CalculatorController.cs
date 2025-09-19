using BukyBookWeb.Helpers;
using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using System.Net;

namespace BukyBookWeb.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorService _calculatorService;
        private readonly ILogger<CalculatorController> _logger;

        public CalculatorController(ICalculatorService calculatorService, ILogger<CalculatorController> logger)
        {
            _calculatorService = calculatorService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                _logger.LogInformation("Calculator page loaded.");
                var model = new CalculatorModel();
                return View(model);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex, "Error loading Calculator page | CorrelationId={LogGuid}", logGuid);
                }

                TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                return this.HandleError(HttpStatusCode.InternalServerError, $"Error loading Calculator page. Tracking ID: {logGuid}");
            }
        }

        [HttpPost]
        public IActionResult Index(CalculatorModel model)
        {
            try
            {
                model.Result = model.Operation switch
                {
                    "Add" => _calculatorService.Add(model.Number1, model.Number2),
                    "Subtract" => _calculatorService.Subtract(model.Number1, model.Number2),
                    "Multiply" => _calculatorService.Multiply(model.Number1, model.Number2),
                    "Divide" => _calculatorService.Divide(model.Number1, model.Number2),
                    _ => throw new InvalidOperationException("Invalid operation selected.")
                };

                _logger.LogInformation(
                    "Calculation successful: {Number1} {Operation} {Number2} = {Result}",
                    model.Number1, model.Operation, model.Number2, model.Result
                );

                return View(model);
            }
            catch (Exception ex)
            {
                var logGuid = Guid.NewGuid();
                using (LogContext.PushProperty("LogGuid", logGuid))
                {
                    _logger.LogError(ex,
                        "Calculation failed for {Number1} {Operation} {Number2} | CorrelationId={LogGuid}",
                        model.Number1, model.Operation, model.Number2, logGuid
                    );
                }

                //TempData["ErrorMessage"] = $"Something went wrong. Tracking ID: {logGuid}";
                model.ErrorMessage = ex.Message;

                return this.HandleError(HttpStatusCode.InternalServerError, $"Calculation error. Tracking ID: {logGuid}");
            }
        }
    }
}
