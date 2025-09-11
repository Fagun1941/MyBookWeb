using BukyBookWeb.Repositories;

namespace BukyBookWeb.Repository
{
    public class CalculatorReposity : ICalculatorRepository
    {
        public double Add(double a, double b)
        {
            try
            {
                return a + b;
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error during addition: " + ex.Message, ex);
            }
        }

        public double Subtract(double a, double b)
        {
            try
            {
                return a - b;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during subtraction: " + ex.Message, ex);
            }
        }

        public double Multiply(double a, double b)
        {
            try
            {
                return a * b;
            }
            catch (Exception ex)
            {
                throw new Exception("Error during multiplication: " + ex.Message, ex);
            }
        }

        public double Divide(double a, double b)
        {
            try
            {
                int x = Convert.ToInt32(a);
                int y = Convert.ToInt32(b);

                return x / y;
            }
            catch (DivideByZeroException ex)
            {
                throw new Exception("Error: Cannot divide by zero.", ex);
            }
            catch (Exception ex)
            {
                throw new Exception("Unexpected error during division: " + ex.Message, ex);
            }
        }


    }
}
