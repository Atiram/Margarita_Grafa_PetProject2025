using AuthenticationService.DAL.Entities;

namespace AuthenticationService.DAL.Repositories.Interfaces;
public interface IUserRepository
{
    Task<UserEntity> GetByIdAsync(string id);
    Task<List<UserEntity>> GetAllAsync();
    Task<UserEntity> CreateAsync(UserEntity user);
    Task<bool> UpdateAsync(string id, UserEntity user);
    Task<bool> DeleteAsync(string id);
    Task<UserEntity> GetUserByUsernameAsync(string username);
}
