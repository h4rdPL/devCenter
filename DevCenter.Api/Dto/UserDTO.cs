using DevCenter.Domain.Enums.Users;

namespace DevCenter.Api.Dto
{
    public record UserDTO(
            string Username,
            string Email,
            string Password,
            UserRoles Role
        );
}
