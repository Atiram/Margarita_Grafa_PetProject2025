using AuthenticationService.API.ViewModels;
using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.Entities;
using Clinic.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController(IAuthService authService, IUserService userService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserRegistrationModel model)
    {
        var user = new UserEntity
        {
            Username = model.Username,
            Password = model.Password,
            Role = model.Role
        };

        var registeredUser = await authService.RegisterAsync(user);
        return Ok(registeredUser);
    }

    [AllowAnonymous]
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginModel model)
    {
        var token = await authService.AuthenticateAsync(model.Username, model.Password);

        if (token != null)
        {
            return Ok(new { Token = token });
        }

        return Unauthorized(NotificationMessages.InvalidAuthErrorMessage);
    }
}
