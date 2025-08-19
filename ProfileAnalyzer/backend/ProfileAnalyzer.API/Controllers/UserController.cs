using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ProfileAnalyzer.API.ViewModels;
using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.Services;
using ProfileAnalyzer.Domain.Interfaces;

namespace ProfileAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController(
        IUserService userService,
        INameAnalysisService nameAnalysisService,
        ILogger<UserController> logger,
        IOptions<JsonSerializerOptions> jsonOptions
    ) : ControllerBase
    {
        private readonly IUserService _userService =
            userService ?? throw new ArgumentNullException(nameof(userService));
        private readonly INameAnalysisService _nameAnalysisService = nameAnalysisService;
        private readonly ILogger<UserController> _logger = logger;
        private readonly JsonSerializerOptions _jsonOptions = jsonOptions.Value;

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
            CreateUserResultViewModel result = new()
            {
                Vowels = numberOfVowels,
                Consonants = numberOfConstants,
                ReversedName = reversedName,
                OriginalData = user,
            };

            _logger.LogInformation(
                "JSON response: \n{JsonResponse}",
                JsonSerializer.Serialize(result, _jsonOptions)
            );
            return Ok(result);
        }
    }
}
