using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.DTOs.Errors;

namespace ProfileAnalyzer.Application.Validation
{
    public static class UserValidator
    {
        public static List<BaseValidationError> Validate(UserDTO user)
        {
            if (user is null)
                throw new ArgumentNullException(nameof(user), "User cannot be null.");

            List<BaseValidationError> errors = [];
            if (string.IsNullOrWhiteSpace(user.FirstName))
                errors.Add(new RequiredFieldError(fieldName: nameof(user.FirstName)));

            if (string.IsNullOrWhiteSpace(user.LastName))
                errors.Add(new RequiredFieldError(fieldName: nameof(user.LastName)));

            if (user.SocialSkills is null || user.SocialSkills.Count == 0)
                errors.Add(new AtLeastOneRequiredError(fieldName: nameof(user.SocialSkills)));

            if (user.SocialAccounts is null || user.SocialAccounts.Count == 0)
                errors.Add(new AtLeastOneRequiredError(fieldName: nameof(user.SocialAccounts)));

            if (
                user.SocialAccounts is not null
                && user.SocialAccounts.Any(account => string.IsNullOrWhiteSpace(account.Address))
            )
                errors.Add(new RequiredFieldError(fieldName: nameof(SocialAccountDTO.Address)));

            return errors;
        }
    }
}
