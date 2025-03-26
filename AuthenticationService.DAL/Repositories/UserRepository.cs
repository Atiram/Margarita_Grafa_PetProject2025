using AuthenticationService.DAL.Entities;
using AuthenticationService.DAL.Repositories.Interfaces;

namespace AuthenticationService.DAL.Repositories;
public class UserRepository : IUserRepository
{
    public List<UserEntity> GetUser()
    {
        List<UserEntity> people = new List<UserEntity> {
        new UserEntity { Username="tom", Password="12345", Role = "admin" },
        new UserEntity { Username="bob", Password="12345", Role = "user" }
        };
        return people;
    }
}
