using System;

namespace TddPlayground
{
    public class FibonacciService : IFibonacci
    {
        public int[] GetElements(int numOfElements)
        {
            if (numOfElements < 2)
            {
                throw new ArgumentOutOfRangeException();
            }

            var elements = new int[numOfElements];
            int a = 0, b = 1;
            elements[0] = a;
            elements[1] = b;
            int i;

            for (i = 2; i < numOfElements; i++) //loop starts from 2 because 0 and 1 are the basis
            {
                elements[i] = a + b;
                a = elements[i - 1];
                b = elements[i];
            }

            return elements;
        }
    }
}