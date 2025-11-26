using RegistrationService.Data;

namespace RegistrationService.Repositories;

public interface IUserRepository
{
    Task<User?> GetUserByEmailAsync(string email);
    Task AddUserAsync(User? user);
}
