using Moq;
using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.Services;
using ProfileAnalyzer.Domain.Entities;
using ProfileAnalyzer.Domain.Enums;
using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.Application.Tests.Services
{
    [TestClass]
    public class UserServiceTests
    {
        private Mock<IRepository>? _repositoryMock;
        private UserService? _service;

        [TestInitialize]
        public void Setup()
        {
            _repositoryMock = new Mock<IRepository>();
            _service = new UserService(_repositoryMock.Object);
        }

        [TestMethod]
        public async Task CreateUser_WhenUserInvalid_ShouldNotCreateUser()
        {
            // Arrange
            UserDTO user = new()
            {
                FirstName = string.Empty,
                LastName = "     ",
                SocialSkills = [],
                SocialAccounts = [],
            };

            // Act
            ApiResponse<int> response = await _service!.CreateUser(user);

            // Assert
            Assert.IsFalse(response.IsSuccess);
            Assert.IsNotNull(response.Errors);
            Assert.IsGreaterThan(0, response.Errors.Count);
            _repositoryMock!.Verify(repo => repo.CreateUser(It.IsAny<User>()), Times.Never);
        }

        [TestMethod]
        public async Task CreateUser_WhenUserValid_ShouldCreateUser()
        {
            // Arrange
            int userId = 11;
            UserDTO user = new()
            {
                FirstName = "John",
                LastName = "Doe",
                SocialSkills = ["C#", "JavaScript"],
                SocialAccounts =
                [
                    new SocialAccountDTO
                    {
                        Type = SocialAccountType.Instagram,
                        Address = "https://wwww.instagram.com",
                    },
                ],
            };
            _repositoryMock!.Setup(repo => repo.CreateUser(It.IsAny<User>())).ReturnsAsync(userId);

            // Act
            ApiResponse<int> response = await _service!.CreateUser(user);

            // Assert
            Assert.IsTrue(response.IsSuccess);
            Assert.AreEqual(userId, response.Data);
            _repositoryMock.Verify(
                repo =>
                    repo.CreateUser(
                        It.Is<User>(u =>
                            u.FirstName == user.FirstName
                            && u.LastName == user.LastName
                            && u.SocialSkills.SequenceEqual(user.SocialSkills)
                            && u.SocialAccounts.Count == user.SocialAccounts.Count
                            && u.SocialAccounts[0].Type == user.SocialAccounts[0].Type
                            && u.SocialAccounts[0].Address == user.SocialAccounts[0].Address
                        )
                    ),
                Times.Once
            );
        }
    }
}
