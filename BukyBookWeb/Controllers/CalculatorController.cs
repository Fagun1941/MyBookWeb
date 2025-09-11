using BukyBookWeb.Models;
using BukyBookWeb.Services;
using Microsoft.AspNetCore.Mvc;

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
            return View(new CalculatorModel());
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
            }
            catch (Exception ex)
            {
                model.ErrorMessage = ex.Message;
            }

            return View(model);
        }

    }
}
