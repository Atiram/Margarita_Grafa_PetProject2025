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
public class AuthService(IUserRepository userRepository) : IAuthService
{
    public async Task<string> AuthenticateAsync(string username, string password)
    {
        try
        {
            var user = await userRepository.GetUserByUsernameAsync(username);

            if (user != null && VerifyPassword(password, user.Password))
            {
                return GenerateJwtToken(user);
            }

            return null;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(string.Format(NotificationMessages.AuthUserErrorMessage, username), ex);
        }
    }

    public async Task<UserEntity> RegisterAsync(UserEntity user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException(NotificationMessages.NoArgumentAuthErrorMessage);
            }

            var hashedPassword = HashPassword(user.Password);
            user.Password = hashedPassword;

            return await userRepository.CreateAsync(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(string.Format(NotificationMessages.RegUserErrorMessage, user.Username), ex);
        }
    }

    private string GenerateJwtToken(UserEntity user)
    {
        var identity = GetIdentity(user.Username, user.Password);
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
            expires: now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME)),
            signingCredentials: new SigningCredentials(
                AuthOptions.GetSymmetricSecurityKey(),
                SecurityAlgorithms.HmacSha256));

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
        return encodedJwt;
    }

    private ClaimsIdentity? GetIdentity(string username, string password)
    {
        var people = userRepository.GetAllAsync().Result;
        UserEntity? person = people.FirstOrDefault(x => x.Username == username && x.Password == password);
        if (person != null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, person.Username),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, person.Role),
            };
            return new ClaimsIdentity(claims, "Token",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
        }
        return null;
    }

    private bool VerifyPassword(string password, string hashedPassword)
    {
        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
    }

    private string HashPassword(string password)
    {
        return BCrypt.Net.BCrypt.HashPassword(password);
    }
}