using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;

namespace DevCenter.Application.Users
{
    public record UserResponseDTO(
            string Username,
            string Email,
            UserRoles UserRoles,
            Company? Company
        );
}
