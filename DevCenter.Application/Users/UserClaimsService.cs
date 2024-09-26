using System.Security.Claims;

namespace DevCenter.Application.Users;

public class UserClaimsService : IUserClaimsService
{
    public Task<string> GetUserEmailClaimAsync(ClaimsPrincipal user)
    {
        return Task.FromResult(user.FindFirst(ClaimTypes.Email)?.Value);
    }
}
