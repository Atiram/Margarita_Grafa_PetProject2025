using AuthenticationService.BLL.Models;

namespace AuthenticationService.BLL.Services.Interfaces;
public interface IUserService
{
    UserModel Get(LoginModel model);
}
