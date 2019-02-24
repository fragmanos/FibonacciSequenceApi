using System;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace TddPlayground.Test
{
    /**
     * In case of fibonacci series, next number is the sum of previous two numbers
     * for example 0, 1, 1, 2, 3, 5, 8, 13, 21 etc.
     * The first two numbers of fibonacci series are 0 and 1.
     */
    
    public class FibonacciServiceTest
    {
        private FibonacciService _fibonacciService;

        [SetUp]
        public void Setup()
        {
            _fibonacciService = new FibonacciService();
        }

        [Test]
        public void GivenPositiveInteger_WhenCalculatingFibonacciSequence_ThenShouldReturnArrayOfElements()
        {
            var expectedElements = new[] {0, 1, 1, 2, 3, 5, 8, 13, 21};
            var actualElements = _fibonacciService.GetElements(9);
            AreEqual(expectedElements, actualElements);
        }

        [TestCase(2, new[] {0, 1})]
        [TestCase(3, new[] {0, 1, 1})]
        [TestCase(5, new[] {0, 1, 1, 2, 3})]
        [TestCase(9, new[] {0, 1, 1, 2, 3, 5, 8, 13, 21})]
        public void GivenPositiveInteger_WhenCalculatingFibonacciSequence_ThenShouldReturnArrayOfElements(
            int numOfElements,
            int[] expectedElements)
        {
            AreEqual(expectedElements, _fibonacciService.GetElements(numOfElements));
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void GivenIntegerLessThanTwo_WhenCalculatingFibonacciSequence_ThenShouldThrowArgumentOutOfRangeException(
            int invalidElementNum)
        {
            Throws<ArgumentOutOfRangeException>(() => _fibonacciService.GetElements(invalidElementNum));
        }
    }
}