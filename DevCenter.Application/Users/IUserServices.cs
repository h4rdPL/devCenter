using DevCenter.Application.Common;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;

namespace DevCenter.Application.Users;

public interface IUserServices
{
    Task<Result<User>> RegisterUser(string username, string email, string password, UserRoles role);
    Task<Result<User>> AuthenticateGoogleUser(string token);
    Task<Result> AddCompanyToUser(int userId, Company company);
    Task<Company?> GetUserCompany(User user);
}
