using System.Security.Claims;

namespace DevCenter.Application.Users
{
    public interface IUserClaimsService
    {
        Task<string> GetUserEmailClaimAsync(ClaimsPrincipal user);
    }
}
