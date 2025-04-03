using AuthenticationService.API.ViewModels;
using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.Entities;
using Clinic.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class UserController(IUserService userService) : ControllerBase
{
    [Authorize]
    [HttpGet("{id}")]
    public async Task<UserEntity> GetUser(string id)
    {

        var user = await userService.GetUserByIdAsync(id);
        return user;

    }

    [Authorize]
    [HttpGet]
    public async Task<List<UserEntity>> GetAllUsers()
    {
        var users = await userService.GetAllUsersAsync();
        return users;
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationModel model)
    {
        var user = new UserEntity
        {
            Username = model.Username,
            Password = model.Password,
            Role = model.Role
        };

        var createdUser = await userService.CreateUserAsync(user);
        return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
    }

    [Authorize]
    [HttpPut]
    public async Task<UserEntity> UpdateUser(string id, [FromBody] UserUpdateModel model)
    {
        var user = new UserEntity
        {
            Id = id,
            Username = model.Username,
            Role = model.Role,
        };

        var updated = await userService.UpdateUserAsync(id, user);
        if (!updated)
        {
            throw new InvalidOperationException(string.Format(NotificationMessages.UpdatingUserErrorMessage, id));
        }
        return user;
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        var deleted = await userService.DeleteUserAsync(id);
        if (deleted)
        {
            return Ok(NotificationMessages.DeletingUserSuccessMessage);
        }

        return NotFound(string.Format(NotificationMessages.DeletingUserErrorMessage, id));
    }
}


