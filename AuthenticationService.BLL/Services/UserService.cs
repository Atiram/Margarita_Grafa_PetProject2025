using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.Entities;
using AuthenticationService.DAL.Repositories.Interfaces;

namespace AuthenticationService.BLL.Services;
public class UserService(IUserRepository userRepository) : IUserService
{
    public async Task<UserEntity> GetUserByIdAsync(string id)
    {
        try
        {
            return await userRepository.GetByIdAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error getting user by Id: {id}", ex);
        }
    }

    public async Task<List<UserEntity>> GetAllUsersAsync()
    {
        try
        {
            return await userRepository.GetAllAsync();
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException("Error getting all users.", ex);
        }
    }

    public async Task<UserEntity> CreateUserAsync(UserEntity user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException("Username and Password are required.");
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;

            return await userRepository.CreateAsync(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error creating user: {user.Username}", ex);
        }
    }

    public async Task<bool> UpdateUserAsync(string id, UserEntity user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username))
            {
                throw new ArgumentException("Username is required for update.");
            }

            return await userRepository.UpdateAsync(id, user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error updating user with Id: {id}", ex);
        }
    }

    public async Task<bool> DeleteUserAsync(string id)
    {
        try
        {
            return await userRepository.DeleteAsync(id);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Error deleting user with Id: {id}", ex);
        }
    }
}
