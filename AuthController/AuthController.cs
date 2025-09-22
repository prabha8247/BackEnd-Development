using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Employee.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Employee.AuthController;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    // AuthDbContext dbContext = null;
    private readonly IConfiguration _configuration;
    
    // public AuthController() : base() {}

    public AuthController(IConfiguration configuration)
    {
        // this.dbContext = dbContext;
        this._configuration  = configuration;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginDto loginDto)
    {

        if (loginDto.Username == "Admin" && loginDto.Password == "123")
        {
            // return Ok("You are logged in!");
            var token = GenerateToken(loginDto.Username,"Admin");
            return Ok(new { token });
        }

        if (loginDto.Username == "User" && loginDto.Password == "123")
        {
            // return Ok("you logged in as user!!");
            var token = GenerateToken(loginDto.Username,"User");
            return Ok(new { token });
        }
        
        return Unauthorized("Invalid Username or Password");
    }
    
    [HttpGet("PublicEndpoint")]
    public IActionResult PublicEndpoint()
    {
        return Ok("This endpoint is public. No login required.");
    }

    public string GenerateToken(string username, string role)
    {
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var claims = new[]
        {

            new Claim(ClaimTypes.Name, username),
            new Claim(ClaimTypes.Role, role)
        };

        var token = new JwtSecurityToken(
            issuer:  _configuration["Jwt:Issuer"],
            audience: _configuration["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );
        
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
    
}