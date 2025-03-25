using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using AuthenticationService.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthenticationService.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private List<User> people = new List<User>
    {
        new User { Username="tom", Password="12345", Role = "admin" },
        new User { Username="bob", Password="12345", Role = "user" }
    };

    [AllowAnonymous]
    [HttpPost("token")]
    [ProducesResponseType(typeof(TokenResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [Consumes("application/x-www-form-urlencoded")]
    public IActionResult Token([FromForm] LoginModel model)
    {
        var identity = GetIdentity(model.Username, model.Password);
        if (identity == null)
        {
            return BadRequest(new { errorText = "Invalid username or password." });
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

        var response = new
        {
            access_token = encodedJwt,
            username = identity.Name
        };

        return Ok(response);
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

    private ClaimsIdentity? GetIdentity(string username, string password)
    {
        User? person = people.FirstOrDefault(x => x.Username == username && x.Password == password);
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
