using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.DTOs.Errors;
using ProfileAnalyzer.Application.Validation;
using ProfileAnalyzer.Domain.Enums;

namespace ProfileAnalyzer.Application.Tests.Validation
{
    [TestClass]
    public class UserValidatorTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Validate_NullUser_ThrowsArgumentNullException()
        {
            // Act & Assert
            UserValidator.Validate(null);
        }

        [TestMethod]
        public void Validate_WhenUserIsInvalid_ReturnsValidationErrors()
        {
            // Arrange
            UserDTO user = new UserDTO
            {
                FirstName = string.Empty,
                LastName = "      ",
                SocialSkills = [],
                SocialAccounts = null,
            };

            // Act
            List<BaseValidationError> errors = UserValidator.Validate(user);

            // Assert
            Assert.IsNotNull(errors);
            Assert.HasCount(4, errors);
            Assert.IsTrue(
                errors.Any(e => e is RequiredFieldError { FieldName: nameof(user.FirstName) })
            );
            Assert.IsTrue(
                errors.Any(e => e is RequiredFieldError { FieldName: nameof(user.LastName) })
            );
            Assert.IsTrue(
                errors.Any(e =>
                    e is AtLeastOneRequiredError { FieldName: nameof(user.SocialSkills) }
                )
            );
            Assert.IsTrue(
                errors.Any(e =>
                    e is AtLeastOneRequiredError { FieldName: nameof(user.SocialAccounts) }
                )
            );
        }

        [TestMethod]
        public void Validate_WhenAdressIsNull_ReturnsError()
        {
            // Arrange
            UserDTO user = new()
            {
                FirstName = "John",
                LastName = "Doe",
                SocialSkills = ["Communication", "Teamwork"],
                SocialAccounts =
                [
                    new SocialAccountDTO
                    {
                        Type = SocialAccountType.Twitter,
                        Address = string.Empty,
                    },
                ],
            };

            // Act
            List<BaseValidationError> errors = UserValidator.Validate(user);

            // Assert
            Assert.IsNotNull(errors);
            Assert.HasCount(1, errors);
            Assert.IsTrue(
                errors.Any(e =>
                    e is RequiredFieldError { FieldName: nameof(SocialAccountDTO.Address) }
                )
            );
        }

        [TestMethod]
        public void Validate_WhenUserIsValid_ReturnsNoErrors()
        {
            // Arrange
            UserDTO user = new()
            {
                FirstName = "John",
                LastName = "Doe",
                SocialSkills = ["Communication", "Teamwork"],
                SocialAccounts =
                [
                    new SocialAccountDTO { Type = SocialAccountType.Twitter, Address = "john_doe" },
                ],
            };
            // Act

            List<BaseValidationError> errors = UserValidator.Validate(user);

            // Assert
            Assert.IsNotNull(errors);
            Assert.IsEmpty(errors);
        }
    }
}
