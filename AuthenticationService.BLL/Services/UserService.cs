using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthenticationService.BLL.Models;
using AuthenticationService.DAL.Entities;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.BLL.Services;
public class UserService
{
    private List<UserEntity> people = new List<UserEntity>
    {
        new UserEntity { Username="tom", Password="12345", Role = "admin" },
        new UserEntity { Username="bob", Password="12345", Role = "user" }
    };
    public void Get(LoginModel model)
    {
        var identity = GetIdentity(model.Username, model.Password);


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

        var response = new
        {
            access_token = encodedJwt,
            username = identity.Name
        };


    }

    private ClaimsIdentity? GetIdentity(string username, string password)
    {
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

    public class TokenResponse
    {
        public string Access_token { get; set; } = "";
        public string Username { get; set; } = "";
    }

    public class ErrorResponse
    {
        public string ErrorText { get; set; } = "";
    }
}
