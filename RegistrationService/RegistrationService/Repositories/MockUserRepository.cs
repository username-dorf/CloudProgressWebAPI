using RegistrationService.Data;

namespace RegistrationService.Repositories;

public class MockUserRepository : IUserRepository
{
    private readonly List<User> _users = new List<User>();

    public Task<User?> GetUserByEmailAsync(string email)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Email == email));
    }

    public Task AddUserAsync(User? user)
    {
        if (user != null)
        {
            _users.Add(user);
        }
        return Task.CompletedTask;
    }
}
