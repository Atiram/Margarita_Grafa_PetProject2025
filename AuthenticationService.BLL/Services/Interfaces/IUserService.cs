using AuthenticationService.DAL.Entities;

namespace AuthenticationService.BLL.Services.Interfaces;
public interface IUserService
{
    Task<UserEntity> GetUserByIdAsync(string id);
    Task<List<UserEntity>> GetAllUsersAsync();
    Task<UserEntity> CreateUserAsync(UserEntity user);
    Task<bool> UpdateUserAsync(string id, UserEntity user);
    Task<bool> DeleteUserAsync(string id);
}
