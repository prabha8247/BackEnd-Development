using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Employee.DTO;
using Employee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace Employee.AuthController;

[ApiController]
[Route("api/[controller]")]
public class AuthController : Controller
{
    // AuthDbContext dbContext = null;
    
    private readonly UserManager<IdentityUser> _userManager;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly IConfiguration _configuration;
    
    // public AuthController() : base() {}

    public AuthController(IConfiguration configuration,  UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
    {
        // this.dbContext = dbContext;
        _configuration  = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {

        // this is before the commit
        
        // if (loginDto.Username == "Admin" && loginDto.Password == "123")
        // {
        //     // return Ok("You are logged in!");
        //     var token = GenerateToken(loginDto.Username,"Admin");
        //     return Ok(new { token });
        // }
        //
        // if (loginDto.Username == "User" && loginDto.Password == "123")
        // {
        //     // return Ok("you logged in as user!!");
        //     var token = GenerateToken(loginDto.Username,"User");
        //     return Ok(new { token });
        // }
        //
        // return Unauthorized("Invalid Username or Password");
        
        var user = await _userManager.FindByNameAsync(loginDto.Username);
        if (user == null) return Unauthorized("Username is incorrect");
        var checkPassword = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!checkPassword) return Unauthorized("password is incorrect");
        
        var roles = await _userManager.GetRolesAsync(user);
        var token = GenerateToken(loginDto.Username, roles.FirstOrDefault());
        return Ok(new { token });

    }
    
    [HttpPost("RegisterUser")]
    public async Task<IActionResult> RegisterUser([FromBody] User pUser)
    {
        if (pUser == null) 
            throw new ArgumentNullException(nameof(pUser));
        
        var user = new IdentityUser {UserName = pUser.Username, Email = pUser.Email};
        var  result = await _userManager.CreateAsync(user, pUser.Password);

        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, pUser.Role);
            return Ok("Data registration successful.");
        }
        
        return BadRequest(result.Errors);
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