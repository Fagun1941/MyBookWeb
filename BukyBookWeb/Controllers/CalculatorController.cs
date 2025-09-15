using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace BukyBookWeb.Controllers
{
    public class CalculatorController : Controller
    {
        private readonly ICalculatorService _calculatorService;

        public CalculatorController(ICalculatorService calculatorService)
        {
            _calculatorService = calculatorService;
        }

        [HttpGet]
        public IActionResult Index()
        {
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
            }
            catch (Exception ex)
            {
                response.Message = $"Error: {ex.Message}";
                response.StatusCode = HttpStatusCode.BadRequest;
                model.ErrorMessage = ex.Message;
                response.Data = model;
            }

            return View(model);
        }
    }
}
