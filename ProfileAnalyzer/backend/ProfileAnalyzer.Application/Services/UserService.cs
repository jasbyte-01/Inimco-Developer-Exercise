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

        public ApiResponse<int> CreateUser(UserDTO userDto)
        {
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

            return ApiResponse<int>.Success(_repository.CreateUser(user));
            ;
        }

        public UserDTO? GetUser(int userId)
        {
            var users = (
                from user in _repository.Users
                join socialAccount in _repository.SocialAccounts
                    on user.UserId equals socialAccount.UserId
                    into socialAccounts
                from socialAccount in socialAccounts.DefaultIfEmpty()
                where user.UserId == userId
                select new
                {
                    user.UserId,
                    user.FirstName,
                    user.LastName,
                    user.SocialSkills,
                    SocialAccountType = socialAccount.Type,
                    SocialAddress = socialAccount.Address,
                }
            ).ToList();

            if (users.Count == 0)
            {
                return null;
            }
            return users
                .GroupBy(u => u.UserId)
                .Select(g => new UserDTO
                {
                    FirstName = g.First().FirstName,
                    LastName = g.First().LastName,
                    SocialSkills = g.First().SocialSkills,
                    SocialAccounts =
                    [
                        .. g.Select(x => new SocialAccountDTO
                        {
                            Type = x.SocialAccountType,
                            Address = x.SocialAddress,
                        }),
                    ],
                })
                .FirstOrDefault();
        }
    }
}
