using DevCenter.Api.Controllers;
using DevCenter.Api.Dto;
using DevCenter.Application.Common;
using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DevCenter.Tests.Controllers;

public partial class UserControllerTests
{
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IConfiguration> _configurationMock;
    private readonly Mock<IUserClaimsService> _userClaimsServiceMock;
    private readonly Mock<IUserServices> _userServicesMock;
    private readonly UserController _controller;

    public UserControllerTests()
    {
        _userRepositoryMock = new Mock<IUserRepository>();
        _configurationMock = new Mock<IConfiguration>();
        _userClaimsServiceMock = new Mock<IUserClaimsService>();
        _userServicesMock = new Mock<IUserServices>();

        _controller = new UserController(
            _userServicesMock.Object,
            _configurationMock.Object,
            _userClaimsServiceMock.Object
        );
    }

    [Fact]
    public async Task AuthenticateGoogle_TokenMissing_ReturnsBadRequest()
    {
        var googleLoginDTO = new GoogleLoginDTO { Token = string.Empty };
        var result = await _controller.AuthenticateGoogle(googleLoginDTO);
        var badRequestResult = result as BadRequestObjectResult;
        badRequestResult.Should().NotBeNull();
        badRequestResult.Value.Should().Be("Token is required.");
    }

    [Fact]
    public async Task AuthenticateGoogle_TokenIsValid_ReturnsOk()
    {
        var mockToken = "mock-valid-google-token";
        var googleLoginDTO = new GoogleLoginDTO { Token = mockToken };

        var mockAuthResult = Result<User>.Success(new User
        {
            Email = "test@example.com",
            Username = "Test User"
        });

        var userServicesMock = new Mock<IUserServices>();
        userServicesMock.Setup(us => us.AuthenticateGoogleUser(mockToken))
                        .ReturnsAsync(mockAuthResult);

        var configurationMock = new Mock<IConfiguration>();
        var userClaimsServiceMock = new Mock<IUserClaimsService>();

        var controller = new UserController(userServicesMock.Object, configurationMock.Object, userClaimsServiceMock.Object);
        var result = await controller.AuthenticateGoogle(googleLoginDTO);

        var okResult = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(200, okResult.StatusCode);
        Assert.Equal(mockAuthResult.Value, okResult.Value);
    }

    [Fact]
    public async Task UserRegister_InvalidEmailFormat_ShouldReturnBadRequest()
    {
        var invalidEmail = "invalid-email-format";
        var userDTO = new UserDTO(
            "TestUser",
            invalidEmail,
            "TestPassword123",
            UserRoles.admin
        );

        var mockResult = Result<User>.Failure("Invalid email format.");

        var userServicesMock = new Mock<IUserServices>();
        userServicesMock.Setup(us => us.RegisterUser(userDTO.Username, userDTO.Email, userDTO.Password, userDTO.Role))
                        .ReturnsAsync(mockResult);

        var configurationMock = new Mock<IConfiguration>();
        var userClaimsServiceMock = new Mock<IUserClaimsService>();

        var controller = new UserController(userServicesMock.Object, configurationMock.Object, userClaimsServiceMock.Object);
        var result = await controller.UserRegister(userDTO);

        var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(actionResult.Result);
        Assert.Equal(400, badRequestResult.StatusCode);
        Assert.Equal("Invalid email format.", badRequestResult.Value);
    }

    [Fact]
    public async Task UserRegister_ValidEmailFormat_ShouldReturnUser()
    {
        var validEmail = "test@example.com";
        var userDTO = new UserDTO(
            "TestUser",
            validEmail,
            "TestPassword123",
            UserRoles.admin
        );

        var mockUser = new User
        {
            Email = validEmail,
            Username = "TestUser",
            Role = UserRoles.admin
        };

        var mockResult = Result<User>.Success(mockUser);

        var userServicesMock = new Mock<IUserServices>();
        userServicesMock.Setup(us => us.RegisterUser(userDTO.Username, userDTO.Email, userDTO.Password, userDTO.Role))
                        .ReturnsAsync(mockResult);

        var configurationMock = new Mock<IConfiguration>();
        var userClaimsServiceMock = new Mock<IUserClaimsService>();

        var controller = new UserController(userServicesMock.Object, configurationMock.Object, userClaimsServiceMock.Object);
        var result = await controller.UserRegister(userDTO);

        var actionResult = Assert.IsType<ActionResult<UserDTO>>(result);
        var createdResult = Assert.IsType<CreatedAtActionResult>(actionResult.Result);

        Assert.Equal(201, createdResult.StatusCode);

        var returnedUser = Assert.IsType<User>(createdResult.Value);
        Assert.Equal(mockUser.Email, returnedUser.Email);
        Assert.Equal(mockUser.Username, returnedUser.Username);
    }

    [Fact]
    public async Task AddCompany_EmptyFields_ShouldReturnBadRequest()
    {
        var company = new Company
        {
            NIP = "",
            Name = "",
            Country = "",
            City = "",
            PostalCode = "",
            Street = "",
            CompanyEmail = ""
        };

        var userId = 1;

        var mockResult = Result.Failure("Company details cannot be empty.");

        _userServicesMock.Setup(us => us.AddCompanyToUser(userId, company))
                         .ReturnsAsync(mockResult);

        var result = await _controller.AddCompany(userId, company);

        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
        badRequestResult.StatusCode.Should().Be(400);
        badRequestResult.Value.Should().Be("Company details cannot be empty.");
    }

    [Fact]
    public async Task AddCompany_ValidDetails_ShouldReturnSuccess()
    {
        var company = new Company
        {
            Name = "Company Name sp. z o o",
            NIP = "01234567890",
            Country = "Poland",
            City = "Warsaw",
            PostalCode = "12345",
            Street = "Słoneczna",
            CompanyEmail = "test@gmail.com"
        };

        var userId = 1;
        var mockResult = Result.Success();

        _userServicesMock.Setup(us => us.AddCompanyToUser(userId, company))
                .ReturnsAsync(mockResult);

        var result = await _controller.AddCompany(userId, company);

        var okResult = Assert.IsType<OkResult>(result);
    }
}
