using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
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
            _logger.LogInformation("Calculator page loaded."); 
            var response = new CommonModel
            {
                Message = "Calculator ready",
                StatusCode = HttpStatusCode.OK,
                Data = new CalculatorModel()
            };

            return View(new CalculatorModel());
        }

        [HttpPost]
        public IActionResult Index(CalculatorModel model)
        {
            var response = new CommonModel();

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

                response.Message = "Calculation successful";
                response.StatusCode = HttpStatusCode.OK;
                response.Data = model;

                _logger.LogInformation(
                    "Calculation successful: {Number1} {Operation} {Number2} = {Result}",
                    model.Number1, model.Operation, model.Number2, model.Result
                );
            }
            catch (Exception ex)
            {
                response.Message = $"Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.BadRequest;
                model.ErrorMessage = ex.Message;
                response.Data = model;

                _logger.LogError(ex,
                    "Calculation failed for {Number1} {Operation} {Number2}",
                    model.Number1, model.Operation, model.Number2
                ); 
            }

            return View(model);
        }
    }
}
