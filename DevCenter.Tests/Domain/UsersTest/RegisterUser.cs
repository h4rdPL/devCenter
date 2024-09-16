using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using DevCenter.Infrastructure.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace DevCenter.Tests.Domain.UsersTest
{
    public class RegisterUser
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly ApplicationDbContext _context;
        private readonly UserServices _userService;

        public RegisterUser()
        {
            _userRepositoryMock = new Mock<IUserRepository>();

            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase("TestDatabase")
                .Options;
            _context = new ApplicationDbContext(options);
            _userService = new UserServices(_userRepositoryMock.Object, _context);
        }

    }
}
