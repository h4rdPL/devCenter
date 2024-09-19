using DevCenter.Api.Controllers;
using DevCenter.Api.Dto;
using DevCenter.Application.Users;
using DevCenter.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DevCenter.Tests.Controllers
{
    public class UserControllerTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUserClaimsService> _userClaimsServiceMock;
        private readonly UserController _controller;
        private readonly Mock<UserServices> _userServicesMock;  


        public UserControllerTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _configurationMock = new Mock<IConfiguration>();
            _userClaimsServiceMock = new Mock<IUserClaimsService>();


            _userServicesMock = new Mock<UserServices>(  
                _userRepositoryMock.Object,
                null 
            );

            _controller = new UserController(
                _userServicesMock.Object,  
                _configurationMock.Object,
                _userClaimsServiceMock.Object
            );
        }


        [Fact]
        public async Task AuthenticateGoogle_TokenMissing_ReturnsBadRequest()
        {
            // Arrange
            var googleLoginDTO = new GoogleLoginDTO { Token = string.Empty };

            // Act
            var result = await _controller.AuthenticateGoogle(googleLoginDTO);

            // Assert
            var badRequestResult = result as BadRequestObjectResult;
            badRequestResult.Should().NotBeNull();
            badRequestResult.Value.Should().Be("Token is required.");
        }
    }
}
