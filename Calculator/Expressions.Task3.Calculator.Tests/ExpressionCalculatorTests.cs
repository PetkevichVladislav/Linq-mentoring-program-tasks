namespace Expressions.Task3.Calculator.Tests
{
    [TestClass]
    public class ExpressionCalculatorTests
    {
        private ExpressionCalculator _calculator;

        public ExpressionCalculatorTests()
        {
            _calculator = new ExpressionCalculator();
        }

        [TestMethod]
        public void Calculate_WhenTestAddition_ShourReturnSum()
        {
            // Arrange
            var expectedResult = 4;
            var expression = "2 + 2";

            // Act
            var result = _calculator.Calculate(expression);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Calculate_WhenTestSubtraction__ShourReturnSum()
        {
            // Arrange
            var expectedResult = 2;
            var expression = "5 - 3";

            // Act
            var result = _calculator.Calculate(expression);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Calculate_WhenTestMultiplication_ShouldReturnProduct()
        {
            // Arrange
            var expectedResult = 12;
            var expression = "3 * 4";

            // Act
            var result = _calculator.Calculate(expression);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Calculate_WhenTestDivision_ShouldReturnProduct()
        {
            // Arrange
            var expectedResult = 5;
            var expression = "10 / 2";

            // Act
            var result = _calculator.Calculate(expression);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Calculate_WhenTestComplexExpression_ShouldReturnCorrectExpressionResult()
        {
            // Arrange
            var expectedResult = 9;
            var expression = "(3+3)*(5-2)/((4-3)*2)";

            // Act
            var result = _calculator.Calculate(expression);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void Calculate_WhenTestInvalidExpression_ShouldReturnError()
        {
            // Arrange
            var expression = "2+";

            // Act & Assert
            Assert.ThrowsException<InvalidOperationException>(() => _calculator.Calculate(expression));
        }
    }
}