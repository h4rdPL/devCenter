using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using System.Text.RegularExpressions;
namespace DevCenter.Application.Users
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;

        public UserServices(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="email"></param>
        /// <param name="password"></param>
        /// <param name="role"></param>
        /// <returns></returns>
        public async Task<Result> RegisterUser(string username, string email, string password, UserRoles role)
        {
            try
            {
                // Validate input parameters
                if (string.IsNullOrEmpty(username))
                    return new Result { Success = false, Message = "Username cannot be empty." };
                if (string.IsNullOrEmpty(email))
                    return new Result { Success = false, Message = "Email cannot be empty." };
                if(!IsValidEmail(email))
                    return new Result { Success = false, Message = "Invalid email format" };
                if (string.IsNullOrEmpty(password))
                    return new Result { Success = false, Message = "Password cannot be empty." };

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
                    Role = role // This is now an enum
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

        private bool IsValidEmail(string email)
        {
            var emailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            return Regex.IsMatch(email, emailRegex);
        }
    }



    public class Result
    {
        public bool Success { get; set; }
        public User Data { get; set; }
        public string Message { get; set; }
    }
}
