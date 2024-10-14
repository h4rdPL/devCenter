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

    /// <summary>
    /// Add User to the database
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>-</returns>
    /// <exception cref="ArgumentNullException">If User is null return exception</exception>
    public async Task Add(User user)
    {
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user), "User cannot be null");
        }

        _context.Add(user);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Getting user by the id
    /// </summary>
    /// <param name="userId">user id </param>
    /// <returns>User entity</returns>
    public async Task<User> GetById(int userId)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
    }

    /// <summary>
    /// Getting user and company using linq expressions
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>User and company entity</returns>
    public async Task<Company?> GetCompanyByUser(User user)
    {
        return await _context.Users
            .Where(u => u.Id == user.Id) 
            .Select(u => u.Company)      
            .FirstOrDefaultAsync();      
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

    /// <summary>
    /// Update user data
    /// </summary>
    /// <param name="user">User entity</param>
    /// <returns>It returns updated user data</returns>
    public async Task Update(User user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
