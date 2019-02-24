using System;
using Microsoft.AspNetCore.Mvc;

namespace TddPlayground
{
    public class FibonacciController : Controller
    {
        private readonly IFibonacci _fibonacci;

        public FibonacciController(IFibonacci fibonacci)
        {
            _fibonacci = fibonacci;
        }

        public IActionResult GetElements(int numOfElements)
        {
            int[] elements;
            if (numOfElements < 2)
            {
                return BadRequest();
            }

            try
            {
                elements = _fibonacci.GetElements(numOfElements);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest();
            }

            return Ok(elements);
        }
    }
}