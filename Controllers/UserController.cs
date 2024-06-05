using Microsoft.AspNetCore.Mvc;
using DotnetCrud.Models;
using DotnetCrud.Services;
using DotnetCrud.DTOs;
using Microsoft.AspNetCore.Authorization;


namespace DotnetCrud.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult> Login([FromBody] LoginForm loginRequest)
        {
            var user = await _userService.LoginUserAsync(loginRequest.Username, loginRequest.Password);
            if (user == null)
            {
                return Unauthorized();
            }

            var token = _userService.GenerateJwtToken(user);
            return Ok(new LoginResponse
            {
                Id = user.Id,
                Username = user.Username,
                Name = user.Name,
                Email = user.Email,
                Token = token,
            });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserView>>> GetUsers([FromQuery] UserFilter filter)
        {
            var users = await _userService.GetUsersAsync(filter);
            return Ok(users);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserView>> GetUser(int id)
        {
            var user = await _userService.GetUserByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }
            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult> CreateUser(User user)
        {
            if (!_userService.IsValidPassword(user.PasswordHash))
            {
                return BadRequest("Password must be at least 8 characters long and contain at least one uppercase letter, one special character, and one number.");
            }

            if (await _userService.UserExistsAsync(user.Username))
            {
                return Conflict("Username already exists.");
            }

            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            await _userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> UpdateUser(int id, User user)
        {
            if (!string.IsNullOrEmpty(user.PasswordHash))
            {
                if (!_userService.IsValidPassword(user.PasswordHash))
                {
                    return BadRequest("Password must be at least 8 characters long and contain at least one uppercase letter, one special character, and one number.");
                }
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
            }

            await _userService.UpdateUserAsync(user);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteUser(int id)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }

        
    }
}
