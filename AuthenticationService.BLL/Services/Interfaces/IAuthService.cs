using AuthenticationService.DAL.Entities;

namespace AuthenticationService.BLL.Services.Interfaces;
public interface IAuthService
{
    Task<string> AuthenticateAsync(string username, string password);
    Task<UserEntity> RegisterAsync(UserEntity user);
}
