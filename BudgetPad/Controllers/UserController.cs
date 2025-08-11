using BudgetPad.Data;
using BudgetPad.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BudgetPad.Controllers
{
  [Route("api/users")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;

    public UserController(AppDbContext context, IConfiguration config)
    {
      _context = context;
      _config = config;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(UserDto request)
    {
      if (_context.Users.Any(user => user.Username == request.Username))
        return BadRequest("User already exists");

      var user = new User
      {
        Username = request.Username,
        PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
        Role = request.Role.ToUpperInvariant(),
        CreatedDate = DateTime.UtcNow,
      };

      _context.Users.Add(user);
      await _context.SaveChangesAsync();

      return Ok("User " + request.Username + " registered.");
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserDto request)
    {
      var user = await _context.Users.SingleOrDefaultAsync(
        user => user.Username == request.Username);

      if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user?.PasswordHash))
        return Unauthorized("Invalid credentials!");

      // Correct credential
      var token = CreateToken(user);
      return Ok(token);
    }

    private string CreateToken(User user)
    {
      var claims = new[]
      {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.Role, user.Role.ToUpperInvariant()),
      };
      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
      var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

      var token = new JwtSecurityToken(
        issuer: _config["Jwt:Issuer"],
        audience: _config["Jwt:Audience"],
        claims: claims,
        expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpiredMinutes"])),
        signingCredentials: creds
      );
      return new JwtSecurityTokenHandler().WriteToken(token);
    }
  }
}
