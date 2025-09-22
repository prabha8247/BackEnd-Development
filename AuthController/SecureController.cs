using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Employee.AuthController;

[ApiController]
[Route("api/[controller]")]
public class SecureController : Controller
{
    private readonly IConfiguration _configuration;

    public SecureController(IConfiguration configuration)
    {
        // this is testing controller
        this._configuration = configuration;
    }
    
    [HttpGet("PublicEndpoint")]
    public IActionResult PublicEndpoint()
    {
        return Ok("This endpoint is public. No login required.");
    }

    [Authorize]
    [HttpGet("ProtectedEndpoint")]
    public IActionResult ProtectedEndpoint()
    {
        return Ok($"Hello {User?.Identity?.Name}, you are authenticated!");
    }

    [Authorize(Roles = "Admin")]
    [HttpGet("AdminEndpoint")]
    public IActionResult AdminEndpoint()
    {
        return Ok("Hello Admin, only users with 'Admin' role can access this.");
    }
}