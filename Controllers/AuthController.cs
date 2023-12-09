using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using crud.ViewModels;
using crud.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;



[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<User> _userManager;
    private readonly SignInManager<User> _signInManager;
    private readonly IConfiguration _configuration;

    public AuthController(UserManager<User> userManager, SignInManager<User> signInManager, IConfiguration configuration)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterViewModel model)
    {
        // Check if the user with the specified UserName already exists
        var userExists = await _userManager.FindByNameAsync(model.UserName) != null;
        if (userExists)
        {
            return BadRequest(new { Message = "Registration failed", Errors = "User with this username already exists" });
        }

        var user = new User { UserName = model.UserName, Name = model.Name };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new { Message = "Registration successful", UserId = user.Id });
        }

        return BadRequest(new { Message = "Registration failed", Errors = result.Errors });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginViewModel model)
    {

        var user = await _userManager.FindByNameAsync(model.UserName);

        if (user == null)
        {
            // User doesn't exist
            return BadRequest(new { Message = "Login failed", Errors = "User not found" });
        }

        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, isPersistent: false, lockoutOnFailure: false);


        if (result.Succeeded)
        {
            var token = GenerateJwtToken(user);
            return Ok(new { Token = token, Message = "Login successful" });
        }

        return BadRequest(new { Message = "Login failed", Errors = "Invalid login attempt" });
    }

    private string GenerateJwtToken(IdentityUser user)
{
    var key = Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]); // Replace with your secret key
    var tokenHandler = new JwtSecurityTokenHandler();

    var tokenDescriptor = new SecurityTokenDescriptor
    {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.Name, user.UserName)
            // Add more claims if needed
        }),
        Expires = DateTime.UtcNow.AddDays(1), // Token expiration time
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    };

    var token = tokenHandler.CreateToken(tokenDescriptor);
    return tokenHandler.WriteToken(token);
}


}
