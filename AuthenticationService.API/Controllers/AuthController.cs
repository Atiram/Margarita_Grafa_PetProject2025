using System.Security.Claims;
using AuthenticationService.BLL.Models;
using AuthenticationService.BLL.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IUserService userService) : ControllerBase
{
    [AllowAnonymous]
    [HttpPost("token")]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult Token([FromForm] LoginModel model)
    {
        UserModel user = userService.Get(model);
        if (user == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
        }

        return Ok(user);
    }

    [Authorize]
    [HttpGet("check")]
    [ProducesResponseType(typeof(object), StatusCodes.Status200OK)]
    public IActionResult Test()
    {
        var identity = User.Identity;
        return Ok(new
        {
            message = "Authentication successful!",
            username = identity?.Name,
            role = User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value
        });
    }
}
