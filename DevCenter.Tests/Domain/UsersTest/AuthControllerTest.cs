using DevCenter.Api.Controllers;
using DevCenter.Application.Users;
using DevCenter.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DevCenter.Tests.Domain.UsersTest
{
    public class UserControllerTest
    {
        private readonly Mock<UserServices> _userServicesMock;
        private readonly UserController _controller;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;

        public UserControllerTest()
        {
            _userServicesMock = new Mock<UserServices>(Mock.Of<IUserRepository>());
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _controller = new UserController(_userServicesMock.Object);

            var httpContext = new DefaultHttpContext
            {
                RequestServices = Mock.Of<IServiceProvider>(s => s.GetService(typeof(IAuthenticationService)) == _authenticationServiceMock.Object)
            };
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }



        /// <summary>
        /// This test verifies that when the Google login process fails to authenticate the user, 
        /// the `GoogleLogin` method returns a `BadRequestObjectResult` with the appropriate error message.
        /// </summary>
        /// <returns></returns>
        [Fact]
        public async Task GoogleLogin_WhenNotAuthenticated_ShouldReturnBadRequest()
        {
            var authResult = AuthenticateResult.Fail("Failed to authenticate");

            _authenticationServiceMock.Setup(x => x.AuthenticateAsync(It.IsAny<HttpContext>(), CookieAuthenticationDefaults.AuthenticationScheme))
                .ReturnsAsync(authResult);

            var result = await _controller.GoogleLogin() as BadRequestObjectResult;

            Assert.NotNull(result);
            Assert.Equal("Failed to authenticate", result.Value);
        }
    }
}
