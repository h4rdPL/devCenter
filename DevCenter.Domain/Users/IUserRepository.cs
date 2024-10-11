using DevCenter.Domain.Entieties;

namespace DevCenter.Domain.Users;

public interface IUserRepository
{
    Task Add(User user);
    Task<User> GetById(int userId);
    Task<Company?> GetCompanyByUser(User user);
    Task<User?> GetUserByEmail(string email);
    Task Update(User user);
}
