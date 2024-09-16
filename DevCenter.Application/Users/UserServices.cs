using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using Google.Apis.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace DevCenter.Application.Users
{
    public class UserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;


        public UserServices(IUserRepository userRepository, ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
                if (string.IsNullOrEmpty(username))
                    return new Result { Success = false, Message = "Username cannot be empty." };
                if (string.IsNullOrEmpty(email))
                    return new Result { Success = false, Message = "Email cannot be empty." };
                if (!IsValidEmail(email))
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

        public async Task<User> AuthenticateGoogleUser(string token)
        {
            try
            {
                // Validate the token
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);

                if (payload != null)
                {
                    Console.WriteLine($"Google Token validated for {payload.Email}");

                    // Check if the user already exists in the database
                    var user = await _userRepository.GetUserByEmail(payload.Email);

                    if (user == null)
                    {
                        Console.WriteLine("Creating new user in the database");

                        user = new User
                        {
                            Email = payload.Email,
                            Username = payload.Name,
                            Role = UserRoles.admin, // Assign a default role
                            Password = "TESTING_PASSWORD",
                            Counter = 0 // Example data
                        };

                        // Add user to the repository
                        await _userRepository.Add(user);

                        // Save changes to the database
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        Console.WriteLine($"User {user.Email} already exists in the database");
                    }

                    return user;
                }
                else
                {
                    Console.WriteLine("Google token validation failed.");
                    throw new Exception("Invalid Google token");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while authenticating Google user: {ex.Message}");
                throw;
            }
        }

        public string GetCurrentUserId()
        {
            // Get the current user's ID from the claims principal
            var userIdClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier);
            return userIdClaim?.Value;
        }


        public async Task<int> GetCounter(string userId)
        {
            Console.WriteLine($"user ID: {userId}");

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return 0;
            }

            Console.WriteLine($"Counter value for user {userId}: {user.Counter}");
            return user.Counter;
        }


        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateCounter(string email, int newCounterValue)
        {
            // Find the user by email instead of ID
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return false;
            }

            // Update the counter
            user.Counter = newCounterValue;
            await _context.SaveChangesAsync();

            Console.WriteLine($"Counter updated for user {user.Email} to {newCounterValue}");
            return true;
        }



        public async Task<int> GetCounterByEmail(string email)
        {
            Console.WriteLine($"Getting counter for user with email: {email}");

            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                Console.WriteLine("User not found.");
                return 0;
            }

            Console.WriteLine($"Counter value for user {email}: {user.Counter}");
            return user.Counter;
        }


        public class Result
        {
            public bool Success { get; set; }
            public User Data { get; set; }
            public string Message { get; set; }
        }
    }
}
