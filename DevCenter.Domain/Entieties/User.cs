using DevCenter.Domain.Enums.Users;

namespace DevCenter.Domain.Entieties;

public class User
{
    public int Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRoles Role { get; set; }
    public int Counter { get; set; }
    public Company Company { get; set; }

}
