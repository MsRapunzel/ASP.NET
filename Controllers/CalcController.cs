using System;
using labWork.Interfaces;

namespace labWork.Controllers
{
    public class CalcController : ICalcService
    {
        public float Add(float a, float b) => a + b;
        public float Substract(float a, float b) => a - b;
        public float Multiply(float a, float b) => a * b;
        public float Divide(float a, float b)
        {
            if (a == 0 || b == 0)
                throw new DivideByZeroException("Error: divided by zero.");

            return a / b;
        }
    }
}

