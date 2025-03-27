using AuthenticationService.API.ViewModels;
using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.Entities;
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
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return user;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error getting user with Id: {id}", ex);
        }
    }

    [Authorize]
    [HttpGet]
    public async Task<List<UserEntity>> GetAllUsers()
    {
        try
        {
            var users = await userService.GetAllUsersAsync();
            return users;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error getting all users.", ex);
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] UserRegistrationModel model)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new UserEntity
            {
                Username = model.Username,
                Password = model.Password,
                Role = model.Role
            };

            var createdUser = await userService.CreateUserAsync(user);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }

    [Authorize]
    [HttpPut]
    public async Task<UserEntity> UpdateUser(string id, [FromBody] UserUpdateModel model)
    {
        try
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
                throw new InvalidOperationException($"Error updating user with Id: {id}");
            }
            return user;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating user with Id: {id}", ex);
        }
    }

    [Authorize]
    [HttpDelete]
    public async Task<IActionResult> DeleteUser(string id)
    {
        try
        {
            var deleted = await userService.DeleteUserAsync(id);
            if (deleted)
            {
                return Ok("User deleted successfully.");
            }

            return NotFound();
        }
        catch (Exception ex)
        {
            return StatusCode(500, "Internal Server Error");
        }
    }
}

