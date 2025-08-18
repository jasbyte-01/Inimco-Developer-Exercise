using Microsoft.AspNetCore.Mvc;
using Moq;
using ProfileAnalyzer.API.Controllers;
using ProfileAnalyzer.API.ViewModels;
using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.DTOs.Errors;
using ProfileAnalyzer.Application.Services;
using ProfileAnalyzer.Domain.Enums;
using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.API.Tests.Controllers
{
    [TestClass]
    public class UserControllerTests
    {
        private Mock<IRepository>? _mockRepository;
        private Mock<IUserService>? _mockUserService;
        private Mock<INameAnalysisService>? _mockNameAnalysisService;
        private UserController? _controller;

        [TestInitialize]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();
            _mockNameAnalysisService = new Mock<INameAnalysisService>();
            _controller = new UserController(
                _mockUserService.Object,
                _mockNameAnalysisService.Object
            );
        }

        [TestMethod]
        public async Task CreateUser_WhenUserIsNull_ShouldReturnBadRequest()
        {
            // Arrange
            UserDTO? user = null;

            // Act
            IActionResult result = await _controller!.CreateUser(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            BadRequestObjectResult? badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.AreEqual("User data cannot be null", badRequestResult.Value);
        }

        [TestMethod]
        public async Task CreateUser_WhenUserIsInvalid_ShouldReturnBadRequest()
        {
            // Arrange
            UserDTO user = new()
            {
                FirstName = "John",
                LastName = "Doe",
                SocialSkills = ["Communication"],
                SocialAccounts = [new() { Type = SocialAccountType.Twitter, Address = "@johndoe" }],
            };
            _mockUserService!
                .Setup(s => s.CreateUser(It.IsAny<UserDTO>()))
                .ReturnsAsync(ApiResponse<int>.Error([new RequiredFieldError("FirstName")]));

            // Act
            IActionResult result = await _controller!.CreateUser(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(BadRequestObjectResult));
            BadRequestObjectResult? badRequestResult = result as BadRequestObjectResult;
            Assert.IsNotNull(badRequestResult);
            Assert.IsInstanceOfType(badRequestResult.Value, typeof(List<BaseValidationError>));
        }

        [TestMethod]
        public async Task CreateUser_WhenUserIsValid_ShouldReturnOkWithAnalysis()
        {
            // Arrange
            int numberOfVowels = 10;
            int numberOfConstants = 30;
            string reversedName = "reversed string";
            UserDTO user = new()
            {
                FirstName = "John",
                LastName = "Doe",
                SocialSkills = ["Communication"],
                SocialAccounts = [new() { Type = SocialAccountType.Twitter, Address = "@johndoe" }],
            };
            _mockUserService!
                .Setup(s => s.CreateUser(It.IsAny<UserDTO>()))
                .ReturnsAsync(ApiResponse<int>.Success(1));
            _mockNameAnalysisService!
                .Setup(s => s.CountVowels(It.IsAny<string>()))
                .Returns(numberOfVowels);
            _mockNameAnalysisService!
                .Setup(s => s.CountConstants(It.IsAny<string>()))
                .Returns(numberOfConstants);
            _mockNameAnalysisService!
                .Setup(s => s.ReverseName(It.IsAny<string>()))
                .Returns(reversedName);

            // Act
            IActionResult result = await _controller!.CreateUser(user);

            // Assert
            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            OkObjectResult? okResult = result as OkObjectResult;
            Assert.IsNotNull(okResult);
            CreateUserResultViewModel? responseData = okResult.Value as CreateUserResultViewModel;
            Assert.IsNotNull(responseData);
            Assert.AreEqual(numberOfVowels, responseData!.Vowels);
            Assert.AreEqual(numberOfConstants, responseData!.Consonants);
            Assert.AreEqual(reversedName, responseData!.ReversedName);
            Assert.AreEqual(user, responseData!.OriginalData);
        }
    }
}
