using ProfileAnalyzer.Application.Services;

namespace ProfileAnalyzer.Application.Tests.Services
{
    [TestClass]
    public class NameAnalysisServiceTests
    {
        private NameAnalysisService _service;

        [TestInitialize]
        public void Setup()
        {
            _service = new NameAnalysisService();
        }

        [TestMethod]
        public void CountVowels_WhenNoVowelPresent_ShouldReturnZero()
        {
            // Arrange
            string text = "bcd1&*\\";

            // Act
            int numberOfVowels = _service.CountVowels(text);

            // Assert
            Assert.AreEqual(0, numberOfVowels);
        }

        [TestMethod]
        public void CountVowels_WhenVowelPresent_ShouldReturnNumberOfVowels()
        {
            // Arrange
            string text = "John Doe";

            // Act
            int numberOfVowels = _service.CountVowels(text);

            // Assert
            Assert.AreEqual(3, numberOfVowels);
        }

        [TestMethod]
        public void CountConstants_WhenNoConstantPresent_ShouldReturnZero()
        {
            // Arrange
            string text = "aeiou";
            // Act
            int numberOfConstants = _service.CountConstants(text);
            // Assert
            Assert.AreEqual(0, numberOfConstants);
        }

        [TestMethod]
        public void CountConstants_WhenConstantPresent_ShouldReturnNumberOfConstants()
        {
            // Arrange
            string text = "John Doe";

            // Act
            int numberOfConstants = _service.CountConstants(text);

            // Assert
            Assert.AreEqual(4, numberOfConstants);
        }

        [TestMethod]
        public void ReverseName_WhenNameIsEmpty_ShouldReturnEmptyString()
        {
            // Arrange
            string name = string.Empty;

            // Act
            string reversedName = _service.ReverseName(name);

            // Assert
            Assert.AreEqual(string.Empty, reversedName);
        }

        [TestMethod]
        public void ReverseName_WhenNameIsSingleWord_ShouldReturnReversedWord()
        {
            // Arrange
            string name = "John Doe";

            // Act
            string reversedName = _service.ReverseName(name);

            // Assert
            Assert.AreEqual("nhoJ eoD", reversedName);
        }
    }
}
