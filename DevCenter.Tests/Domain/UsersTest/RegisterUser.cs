using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
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
            Role = UserRoles.Employee
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

    /// <summary>
    /// This test verifies that when a valid user is created, the `RegisterUser` method returns a success message.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RegisterUser_WhenValidUserRoleIsProvided_ShouldRegisterUserSuccessfully()
    {
        // Arrange
        var email = "test@gmail.com";
        var userRole = UserRoles.Employee; // Using the UserRole enum
        var user = new User
        {
            Username = "Test",
            Email = email,
            Password = "123",
            Role = userRole
        };

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(email))
                           .ReturnsAsync((User)null); // No user exists with this email

        _userRepositoryMock.Setup(repo => repo.Add(It.Is<User>(u => u.Email == email && u.Role == userRole)))
                           .Returns(Task.CompletedTask);

        // Act
        var result = await _userService.RegisterUser(user.Username, user.Email, user.Password, user.Role);

        // Assert
        Assert.True(result.Success);
        Assert.Equal("User registered successfully.", result.Message);
        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(email), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Add(It.Is<User>(u => u.Email == email && u.Role == userRole)), Times.Once);
    }

    /// <summary>
    /// This test verifies that when a valid user role is provided, the `RegisterUser` method registers the user successfully.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RegisterUser_WhenEmailIsInvalid_ShouldReturnFalseMessage()
    {
        var email = "invalid.email";
        var userRole = UserRoles.admin;

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))  
            .ReturnsAsync((User)null);


        var result = _userService.RegisterUser("TestUser", email, "password", userRole);

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Never);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Never);
    }

    /// <summary>
    /// This test verifies that when an invalid email format is provided, the `RegisterUser` method returns a failure message and does not proceed with user registration.
    /// </summary>
    /// <returns></returns>
    [Fact]
    public async Task RegisterUser_WhenEmailIsValid_ShouldCreateUser()
    {
        var email = "email@gmail.com";
        var userRole = UserRoles.admin;

        _userRepositoryMock.Setup(repo => repo.GetUserByEmail(It.IsAny<string>()))
            .ReturnsAsync((User)null);

        var result = _userService.RegisterUser("TestUser", email, "password", userRole);

        _userRepositoryMock.Verify(repo => repo.GetUserByEmail(It.IsAny<string>()), Times.Once);
        _userRepositoryMock.Verify(repo => repo.Add(It.IsAny<User>()), Times.Once);
    }

}