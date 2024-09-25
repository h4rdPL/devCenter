using DevCenter.Application.Users;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace DevCenter.Tests.Domain.UsersTest
{
    public class RegisterUser
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ApplicationDbContext _context;
        private readonly UserServices _userService;
        private readonly Mock<IPasswordHasher> _passwordHasherMock;

        public RegisterUser()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _passwordHasherMock = new Mock<IPasswordHasher>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _userService = new UserServices(_userRepositoryMock.Object, _context, _passwordHasherMock.Object);
        }

    }
}
