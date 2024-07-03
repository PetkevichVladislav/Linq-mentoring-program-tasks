namespace Expressions.Task3.StringExtensions.Tests
{
    [TestClass]
    public class StringExtensionTests
    {

        [TestMethod]
        public void ExtractDigit_WhenLineContainsDigits_ShourReturnDigits()
        {
            // Arrange
            var line = "The high-speed train traveled at 300 km per hour";
            var expectedResult = "300";

            // Act
            var result = line.ExtractDigits();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ExtractDigit_WhenLineDoesNotContainDigits_ShourReturnEmptyString()
        {
            // Arrange
            var line = "The high-speed train traveled at fast speed";
            var expectedResult = string.Empty;

            // Act
            var result = line.ExtractDigits();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ExtractDigit_WhenLineIsEmpty_ShourReturnEmptyString()
        {
            // Arrange
            var line = "";
            var expectedResult = string.Empty;

            // Act
            var result = line.ExtractDigits();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }


        [TestMethod]
        public void ExtractDigit_WhenLineContainsDigitsAndSpecialCharacters_ShourReturnDigits()
        {
            // Arrange
            var line = "The high-speed train \\\"火車\\\" traveled at 1500 km/h at 2001.";
            var expectedResult = "15002001";

            // Act
            var result = line.ExtractDigits();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void RemoveTrailingSpaces_WhenLineContainsTrailingSpacesAtStart_ShourReturnLineWithoutTrailingSpaces()
        {
            // Arrange
            var line = "    pingpong";
            var expectedResult = " pingpong";

            // Act
            var result = line.RemoveTrailingSpaces();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void RemoveTrailingSpaces_WhenLineContainsTrailingSpacesAtCenter_ShourReturnLineWithoutTrailingSpaces()
        {
            // Arrange
            var line = "ping        pong";
            var expectedResult = "ping pong";

            // Act
            var result = line.RemoveTrailingSpaces();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void RemoveTrailingSpaces_WhenLineContainsTrailingSpacesAtEnd_ShourReturnLineWithoutTrailingSpaces()
        {
            // Arrange
            var line = "pingpong      ";
            var expectedResult = "pingpong ";

            // Act
            var result = line.RemoveTrailingSpaces();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void RemoveTrailingSpaces_WhenLineContainsTrailingSpaces_ShourReturnLineWithoutTrailingSpaces()
        {
            // Arrange
            var line = "The rhythmic    sound of the pingpong   ball bouncing back and     forth echoed through the       room as the    players engaged in     an intense match.";
            var expectedResult = "The rhythmic sound of the pingpong ball bouncing back and forth echoed through the room as the players engaged in an intense match.";

            // Act
            var result = line.RemoveTrailingSpaces();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void RemoveTrailingSpaces_WhenLineContainsOnlySpaces_ShourReturnLineWithSpaces()
        {
            // Arrange
            var line = "    ";
            var expectedResult = " ";

            // Act
            var result = line.RemoveTrailingSpaces();

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}