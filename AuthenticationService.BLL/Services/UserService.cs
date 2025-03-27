using AuthenticationService.BLL.Services.Interfaces;
using AuthenticationService.DAL.Entities;
using AuthenticationService.DAL.Repositories.Interfaces;
using Clinic.Domain;

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
            throw new InvalidOperationException(string.Format(NotificationMessages.GettingUserErrorMessage, id), ex);
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
            throw new InvalidOperationException(NotificationMessages.GettingAllUserErrorMessage);
        }
    }

    public async Task<UserEntity> CreateUserAsync(UserEntity user)
    {
        try
        {
            if (string.IsNullOrEmpty(user.Username) || string.IsNullOrEmpty(user.Password))
            {
                throw new ArgumentException(NotificationMessages.NoArgumentAuthErrorMessage);
            }
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = hashedPassword;

            return await userRepository.CreateAsync(user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(string.Format(NotificationMessages.CreatingUserErrorMessage, user.Username), ex);
        }
    }

    public async Task<bool> UpdateUserAsync(string id, UserEntity user)
    {
        try
        {
            return await userRepository.UpdateAsync(id, user);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException(string.Format(NotificationMessages.UpdatingUserErrorMessage, id), ex);
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
            throw new InvalidOperationException(string.Format(NotificationMessages.DeletingUserErrorMessage, id), ex);
        }
    }
}
