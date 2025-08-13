using Microsoft.AspNetCore.Mvc;
using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.Services;

namespace ProfileAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(UserService userService, INameAnalysisService nameAnalysisService)
        : ControllerBase
    {
        private readonly UserService _userService =
            userService ?? throw new ArgumentNullException(nameof(userService));
        private readonly INameAnalysisService _nameAnalysisService = nameAnalysisService;

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] UserDTO user)
        {
            if (user is null)
            {
                return BadRequest("User data cannot be null");
            }

            ApiResponse<int> response = await _userService.CreateUser(user);
            if (response.IsFailure)
            {
                return BadRequest(response.Errors);
            }

            string fullName = $"{user.FirstName} {user.LastName}";
            int numberOfVowels = _nameAnalysisService.CountVowels(fullName);
            int numberOfConstants = _nameAnalysisService.CountConstants(fullName);
            string reversedName = _nameAnalysisService.ReverseName(fullName);

            return Ok(
                new
                {
                    Vowels = numberOfVowels,
                    Consonants = numberOfConstants,
                    ReversedName = reversedName,
                    OriginalData = user,
                }
            );
        }
    }
}
