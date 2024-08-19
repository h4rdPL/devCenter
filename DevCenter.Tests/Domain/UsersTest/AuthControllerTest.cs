using System.Security.Claims;
using System.Threading.Tasks;
using DevCenter.Api.Controllers;
using DevCenter.Application.Users;
using DevCenter.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace DevCenter.Tests.Domain.UsersTest
{
    public class UserControllerTest
    {
        private readonly Mock<UserServices> _userServicesMock;
        private readonly UserController _controller;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public UserControllerTest()
        {
            // Initialize mocks
            _userServicesMock = new Mock<UserServices>(Mock.Of<IUserRepository>());
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            // Initialize controller with the mock service
            _controller = new UserController(_userServicesMock.Object);

            // Set up HttpContext with mocked services
            var httpContext = new DefaultHttpContext
            {
                RequestServices = Mock.Of<IServiceProvider>(s => s.GetService(typeof(IAuthenticationService)) == _authenticationServiceMock.Object)
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }




        [Fact]
        public async Task GoogleLogin_WhenNotAuthenticated_ShouldReturnBadRequest()
        {
            // Arrange
            var authResult = AuthenticateResult.Fail("Failed to authenticate");

            // Setup the mock to return the result when AuthenticateAsync is called
            _authenticationServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme))
                .ReturnsAsync(authResult);

            // Act
            var result = await _controller.GoogleLogin() as BadRequestObjectResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Failed to authenticate", result.Value);
        }
    }
}
