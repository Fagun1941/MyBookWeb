using BukyBookWeb.Repositories;

namespace BukyBookWeb.Services
{
    public class CalculatorService : ICalculatorService
    {
        private readonly ICalculatorRepository _calculatorRepo;

        public CalculatorService(ICalculatorRepository calculatorRepo)
        {
            _calculatorRepo = calculatorRepo;
        }

        public double Add(double a, double b)
        {
            try
            {
                return _calculatorRepo.Add(a, b);
            }
            catch (Exception ex)
            {
                throw new Exception("Service error during addition: " + ex.Message, ex);
            }
        }

        public double Subtract(double a, double b)
        {
            try
            {
                return _calculatorRepo.Subtract(a, b);
            }
            catch (Exception ex)
            {
                throw new Exception("Service error during subtraction: " + ex.Message, ex);
            }
        }

        public double Multiply(double a, double b)
        {
            try
            {
                return _calculatorRepo.Multiply(a, b);
            }
            catch (Exception ex)
            {
                throw new Exception("Service error during multiplication: " + ex.Message, ex);
            }
        }

        public double Divide(double a, double b)
        {
            try
            {
                return _calculatorRepo.Divide(a, b);
            }
            catch (DivideByZeroException ex)
            {
                throw new DivideByZeroException("Cannot divide by zero in service layer.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Service error during division: " + ex.Message, ex);
            }
        }
    }
}
