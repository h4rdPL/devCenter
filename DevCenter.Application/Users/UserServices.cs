using DevCenter.Domain.Entieties;
using DevCenter.Domain.Users;
namespace DevCenter.Application.Users
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<Result> RegisterUser(string username, string email, string password, string role)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(username))
                    return new Result { Success = false, Message = "Username cannot be empty." };

                if (string.IsNullOrEmpty(email))
                    return new Result { Success = false, Message = "Email cannot be empty." };

                if (string.IsNullOrEmpty(password))
                    return new Result { Success = false, Message = "Password cannot be empty." };

                if (string.IsNullOrEmpty(role))
                    return new Result { Success = false, Message = "Role cannot be empty." };

                // Check if user already exists
                var existingUser = await _userRepository.GetUserByEmail(email);
                if (existingUser != null)
                    return new Result { Success = false, Message = "A user with this email already exists." };

                // Hash the password
                var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                // Create a new user
                var user = new User
                {
                    Username = username,
                    Email = email,
                    Password = hashedPassword,
                    Role = role
                };

                // Add the user to the repository
                await _userRepository.Add(user);

                return new Result { Success = true, Data = user, Message = "User registered successfully." };
            }
            catch (Exception ex)
            {
                // Log the exception as needed
                return new Result { Success = false, Message = $"Internal server error: {ex.Message}" };
            }
        }
    }

    public class Result
    {
        public bool Success { get; set; }
        public User Data { get; set; }
        public string Message { get; set; }
    }
}
