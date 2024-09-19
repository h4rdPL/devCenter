using DevCenter.Api.Controllers;
using DevCenter.Application.Users;
using DevCenter.Domain.Users;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Moq;

namespace DevCenter.Tests.Domain.UsersTest
{
    public class UserControllerTest
    {
        private readonly Mock<UserServices> _userServicesMock;
        private readonly Mock<IConfiguration> _configurationMock;
        private readonly Mock<IUserClaimsService> _userClaimsServiceMock;
        private readonly Mock<IAuthenticationService> _authenticationServiceMock;
        private readonly UserController _controller;

        public UserControllerTest()
        {
            _userServicesMock = new Mock<UserServices>(Mock.Of<IUserRepository>());
            _configurationMock = new Mock<IConfiguration>();
            _userClaimsServiceMock = new Mock<IUserClaimsService>();
            _authenticationServiceMock = new Mock<IAuthenticationService>();

            _controller = new UserController(
                _userServicesMock.Object,
                _configurationMock.Object,
                _userClaimsServiceMock.Object
            );

            var httpContext = new DefaultHttpContext
            {
                RequestServices = Mock.Of<IServiceProvider>(s => s.GetService(typeof(IAuthenticationService)) == _authenticationServiceMock.Object)
            };

            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = httpContext
            };
        }

    }
}
