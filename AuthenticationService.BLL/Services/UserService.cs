using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthenticationService.BLL.Models;
using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.Entities;
using AuthenticationService.DAL.Repositories.Interfaces;
using Clinic.Domain;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.BLL.Services;
public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserEntity> GetUserByIdAsync(string id)
    {
        try
        {
            return await userRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error getting user by Id: {id}", ex);
        }
    }

    public async Task<List<UserEntity>> GetAllUsersAsync()
    {
        try
        {
            return await userRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error getting all users.");
        }
    }

    public async Task<UserEntity> CreateUserAsync(UserEntity user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Username and Password are required.");
            }
            //var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            //user.Password = hashedPassword;

            return await userRepository.CreateAsync(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error creating user: {user.Username}", ex);
        }
    }

    public async Task<bool> UpdateUserAsync(string id, UserEntity user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentException("Username is required for update.");
            }

            return await userRepository.UpdateAsync(id, user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating user with Id: {id}", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        try
        {
            return await userRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting user with Id: {id}", ex);
        }
    }



    public UserModel Get(LoginModel model)
    {
        var identity = GetIdentity(model.Username, model.Password);
        if (identity == null)
        {
            throw new ValidationException(NotificationMessages.InvalidAuthErrorMessage);
        }
        var now = DateTime.UtcNow;
        var jwt = new JwtSecurityToken(
            issuer: AuthOptions.ISSUER,
            audience: AuthOptions.AUDIENCE,
            notBefore: now,
            claims: identity.Claims,
            expires: now.Add(TimeSpan.FromSeconds(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        //var t = userRepository.GetAllAsync();
        return new UserModel()
        {
            Username = identity.Name,
            Token = encodedJwt,
        };
    }

    private ClaimsIdentity? GetIdentity(string username, string password)
    {
        var people = userRepository.GetUser();
        var c = people.Result;
        UserEntity? person = c.FirstOrDefault(x => x.Username == username && x.Password == password);
        if (person != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role)
            };
            return new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        }
        return null;
    }
}
