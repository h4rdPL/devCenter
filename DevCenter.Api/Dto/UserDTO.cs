namespace DevCenter.Api.Dto
{
    public record UserDTO(
            string Username,
            string Email,
            string Password,
            string Role
        );
}
