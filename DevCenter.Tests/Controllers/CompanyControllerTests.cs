using DevCenter.Api.Controllers;
using DevCenter.Application.Common;
using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Enums.Users;
using DevCenter.Domain.Users;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Security.Claims;

namespace DevCenter.Tests.Controllers
{
    public class CompanyControllerTests
    {
        private readonly Mock<IUserServices> _userServicesMock;
        private readonly Mock<IUserClaimsService> _userClaimsServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _userServicesMock = new Mock<IUserServices>();
            _userClaimsServiceMock = new Mock<IUserClaimsService>();
            _userRepositoryMock = new Mock<IUserRepository>();

            _controller = new CompanyController(_userServicesMock.Object, _userClaimsServiceMock.Object, _userRepositoryMock.Object);
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
            var emailClaim = "test@example.com";

            _userClaimsServiceMock.Setup(ucs => ucs.GetUserEmailClaimAsync(It.IsAny<ClaimsPrincipal>()))
                                  .ReturnsAsync(emailClaim);

            var user = new User
            {
                Id = userId,
                Email = emailClaim,
                Username = "TestUser",
                Role = UserRoles.admin
            };

            _userRepositoryMock.Setup(ur => ur.GetUserByEmail(emailClaim))
                               .ReturnsAsync(user);

            var mockResult = Result.Failure("Company details cannot be empty.");
            _userServicesMock.Setup(us => us.AddCompanyToUser(userId, company))
                             .ReturnsAsync(mockResult);

            var result = await _controller.AddCompany(company);

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
                NIP = "0123456789",
                Country = "Poland",
                City = "Warsaw",
                PostalCode = "12345",
                Street = "Słoneczna",
                CompanyEmail = "test@gmail.com"
            };

            var userId = 1; 
            var emailClaim = "test@gmail.com";

            _userClaimsServiceMock.Setup(ucs => ucs.GetUserEmailClaimAsync(It.IsAny<ClaimsPrincipal>()))
                                  .ReturnsAsync(emailClaim);

            var user = new User
            {
                Id = 1,
                Email = emailClaim,
                Username = "TestUser",
                Role = UserRoles.admin
            };

            _userRepositoryMock.Setup(ur => ur.GetUserByEmail(emailClaim))
                               .ReturnsAsync(user);

            var mockResult = Result.Success();
            _userServicesMock.Setup(us => us.AddCompanyToUser(userId, company))
                             .ReturnsAsync(mockResult);

            var result = await _controller.AddCompany(company);

            var okResult = Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public async Task AddCompany_InvalidNIP_ShouldReturnBadRequest()
        {
            var company = new Company
            {
                Name = "Test Company",
                NIP = "12345",
                Country = "Poland",
                City = "Warsaw",
                PostalCode = "12345",
                Street = "Słoneczna",
                CompanyEmail = "test@example.com"
            };

            var result = await _controller.AddCompany(company);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("NIP must be a 10 digit number.");
        }

        [Fact]
        public async Task HasCompany_ShouldReturnTrue_WhenUserHasCompany()
        {
            var userId = 1;

            _userServicesMock.Setup(s => s.UserHasCompany(userId))
                             .ReturnsAsync(true);

            var result = await _controller.HasCompany() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.True((bool)result.Value);

        }

        [Fact]
        public async Task HasCompany_ShouldReturnFalse_WhenUserDoesNotHaveCompany()
        {
            var userId = 1;

            _userServicesMock.Setup(s => s.UserHasCompany(userId)).ReturnsAsync(false);

            var result = await _controller.HasCompany() as OkObjectResult;

            Assert.NotNull(result);
            Assert.Equal(200, result.StatusCode);
            Assert.False((bool)result.Value);   

        }
    }
}
