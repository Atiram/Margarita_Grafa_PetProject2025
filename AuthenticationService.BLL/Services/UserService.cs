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

        return new UserModel()
        {
            Username = identity.Name,
            Token = encodedJwt,
        };
    }

    private ClaimsIdentity? GetIdentity(string username, string password)
    {
        var people = userRepository.GetUser();
        UserEntity? person = people.FirstOrDefault(x => x.Username == username && x.Password == password);
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
