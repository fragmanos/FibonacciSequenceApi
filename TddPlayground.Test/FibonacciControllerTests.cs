using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using static NUnit.Framework.Assert;

namespace TddPlayground.Test
{
    public class FibonacciControllerTests
    {
        private FibonacciController _fibonacciController;
        private Mock<IFibonacci> _mockFibonacciService;

        [SetUp]
        public void Setup()
        {
            _mockFibonacciService = new Mock<IFibonacci>();
            _fibonacciController = new FibonacciController(_mockFibonacciService.Object);
        }

        [Test]
        public void
            GivenPositiveInteger_WhenReceivesValidArrayFromService_ThenShouldReturnHttpStatus200_AndArrayOfFibonacciElements()
        {
            var expected = new[] {0, 1, 1, 2, 3, 5, 8, 13, 21};
            const int numOfElements = 9;
            _mockFibonacciService.Setup(m => m.GetElements(9)).Returns(expected);
            var response = (OkObjectResult) _fibonacciController.GetElements(numOfElements);

            AreEqual(StatusCodes.Status200OK, response.StatusCode);
            AreEqual(expected, response.Value);
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public void
            GivenIntegerLessThanTwo_WhenProcessed_ThenShouldReturnHttpStatus400(int invalidElementNum)
        {
            var response = (BadRequestResult) _fibonacciController.GetElements(invalidElementNum);
            AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }

        [Test]
        public void GivenExceptionFromService_WhenProcessValidInteger_ThenShouldReturnHttpStatus400()
        {
            _mockFibonacciService.Setup(m => m.GetElements(It.IsAny<int>())).Throws<ArgumentOutOfRangeException>();
            var response = (BadRequestResult) _fibonacciController.GetElements(9);
            AreEqual(StatusCodes.Status400BadRequest, response.StatusCode);
        }
    }
}