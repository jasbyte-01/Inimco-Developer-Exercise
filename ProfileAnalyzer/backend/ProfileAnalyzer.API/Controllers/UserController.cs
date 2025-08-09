using Microsoft.AspNetCore.Mvc;
using ProfileAnalyzer.Application.DTOs;
using ProfileAnalyzer.Application.Services;

namespace ProfileAnalyzer.API.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly UserService _userService;

        public UserController(UserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpPost]
        public IActionResult CreateUser([FromBody] UserDTO user)
        {
            if (user is null)
            {
                return BadRequest("User data cannot be null");
            }

            ApiResponse<int> response = _userService.CreateUser(user);
            if (response.IsFailure)
            {
                return BadRequest(response.Errors);
            }

            return Ok($"User created successfully with ID: {response.Data}");
        }

        [HttpGet]
        public IActionResult GetUser(int userId)
        {
            UserDTO? user = _userService.GetUser(userId);
            if (user is null)
            {
                return NotFound($"User with ID: {userId} does not exist");
            }

            return Ok(user);
        }
    }
}
