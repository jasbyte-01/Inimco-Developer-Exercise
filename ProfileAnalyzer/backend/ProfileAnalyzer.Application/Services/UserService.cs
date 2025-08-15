using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.DTOs.Errors;
using ProfileAnalyzer.Application.Validation;
using ProfileAnalyzer.Domain.Entities;
using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.Application.Services
{
    public class UserService(IRepository repository)
    {
        private readonly IRepository _repository =
            repository ?? throw new ArgumentNullException(nameof(repository));

        public async Task<ApiResponse<int>> CreateUser(UserDTO userDto)
        {
            // Validate the user data
            List<BaseValidationError> validationErrors = UserValidator.Validate(userDto);
            if (validationErrors.Count != 0)
            {
                return ApiResponse<int>.Error(validationErrors);
            }

            User user = new()
            {
                FirstName = userDto.FirstName,
                LastName = userDto.LastName,
                SocialSkills = userDto.SocialSkills,
                SocialAccounts = userDto.SocialAccounts.ConvertAll(
                    socialAccount => new SocialAccount
                    {
                        Type = socialAccount.Type,
                        Address = socialAccount.Address,
                    }
                ),
            };

            // Create the user in the repository
            int userId = await _repository.CreateUser(user);
            return ApiResponse<int>.Success(userId);
        }
    }
}
