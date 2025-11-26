using Microsoft.EntityFrameworkCore;
using RegistrationService.Data;

namespace RegistrationService.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<User?> GetUserByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task AddUserAsync(User? user)
    {
        if (user == null) throw new ArgumentNullException(nameof(user), "User cannot be null");

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}