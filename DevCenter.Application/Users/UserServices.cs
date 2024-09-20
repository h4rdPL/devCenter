using DevCenter.Application.Common;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using Google.Apis.Auth;
using Microsoft.EntityFrameworkCore;
namespace DevCenter.Application.Users
{
    public class UserServices : IUserServices
    {
        private readonly IUserRepository _userRepository;
        private readonly ApplicationDbContext _context;

        public UserServices(IUserRepository userRepository, ApplicationDbContext context)
        {
            _userRepository = userRepository;
            _context = context;
        }

        public async Task<Result<User>> RegisterUser(string username, string email, string password, UserRoles role)
        {
            if (string.IsNullOrEmpty(username)) return Result<User>.Failure("Username cannot be empty.");
            if (string.IsNullOrEmpty(email)) return Result<User>.Failure("Email cannot be empty.");
            if (!IsValidEmail(email)) return Result<User>.Failure("Invalid email format.");
            if (string.IsNullOrEmpty(password)) return Result<User>.Failure("Password cannot be empty.");

            var existingUser = await _userRepository.GetUserByEmail(email);
            if (existingUser != null) return Result<User>.Failure("A user with this email already exists.");

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            var user = new User
            {
                Username = username,
                Email = email,
                Password = hashedPassword,
                Role = role
            };

            await _userRepository.Add(user);
            return Result<User>.Success(user);
        }

        public async Task<Result<User>> AuthenticateGoogleUser(string token)
        {
            try
            {
                var payload = await GoogleJsonWebSignature.ValidateAsync(token);
                if (payload == null) return Result<User>.Failure("Invalid Google token.");

                var user = await _userRepository.GetUserByEmail(payload.Email);
                if (user == null)
                {
                    user = new User
                    {
                        Email = payload.Email,
                        Username = payload.Name,
                        Role = UserRoles.admin,
                        Password = "TESTING_PASSWORD",
                        Counter = 0
                    };
                    await _userRepository.Add(user);
                    await _context.SaveChangesAsync();
                }

                return Result<User>.Success(user);
            }
            catch (Exception ex)
            {
                return Result<User>.Failure($"Error while authenticating Google user: {ex.Message}");
            }
        }

        public async Task<Result<int>> GetCounterByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return Result<int>.Failure("User not found.");

            return Result<int>.Success(user.Counter);
        }

        public async Task<Result> UpdateCounter(string email, int newCounterValue)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user == null) return Result.Failure("User not found.");

            user.Counter = newCounterValue;
            await _context.SaveChangesAsync();
            return Result.Success();
        }

        private bool IsValidEmail(string email)
        {
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
    }
}