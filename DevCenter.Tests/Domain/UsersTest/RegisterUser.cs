using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Users;
using Moq;

namespace DevCenter.Tests.Domain.UsersTest;

public class RegisterUser
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly UserServices _userService;

    public RegisterUser()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _userService = new UserServices(_userRepositoryMock.Object);
    }


    [Fact]
    public async Task RegisterUser_WhenValidUserIsCreated_ShouldReturnSuccessMessage()
    {
        // Arrange
        var email = "test@gmail.com";
        var user = new User
        {
            Username = "Test",
            Email = email,
            Password = "123",
            Role = "Employee"
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
                           .ReturnsAsync((User)null);

        _userRepositoryMock.Setup(repo => repo.Add(It.Is<User>(u => u.Email == email)))
                           .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.RegisterUser(user.Username, user.Email, user.Password, user.Role);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("User registered successfully.", result.Message);
        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Add(It.Is<User>(u => u.Email == email)), Times.Once);

    }
}