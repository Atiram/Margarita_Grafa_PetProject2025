using AuthenticationService.DAL.Entities;

namespace AuthenticationService.DAL.Repositories.Interfaces;
public interface IUserRepository
{
    List<UserEntity> GetUser();
}
