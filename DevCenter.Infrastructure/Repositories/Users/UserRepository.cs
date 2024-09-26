using DevCenter.Domain.Entieties;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace DevCenter.Infrastructure.Repositories.Users;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _context;

    public UserRepository(ApplicationDbContext context)
    {
        _context = context;
    }


    public async Task Add(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        _context.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task<User> GetById(int userId)
    {
        return await _context.Users.FindAsync(userId);
    }

    /// <summary>
    /// Retrieves a <see cref="User"/> entity from the database based on the provided email address.
    /// </summary>
    /// <param name="email">The email address of the user to retrieve.</param>
    /// <returns>
    /// A <see cref="Task{TResult}"/> representing the asynchronous operation. The task result contains
    /// the <see cref="User"/> entity if found; otherwise, <c>null</c>.
    /// </returns>
    /// <exception cref="ArgumentException">
    /// Thrown when the <paramref name="email"/> is null or empty.
    /// </exception>
    public async Task<User?> GetUserByEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            throw new ArgumentException("Email cannot be null or empty", nameof(email));
        }

        return await _context.Set<User>()
                             .AsNoTracking()
                             .FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
