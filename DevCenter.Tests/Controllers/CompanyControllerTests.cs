using DevCenter.Api.Controllers;
using DevCenter.Application.Common;
using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace DevCenter.Tests.Controllers
{
    public class CompanyControllerTests
    {
        private readonly Mock<IUserServices> _userServicesMock;
        private readonly CompanyController _controller;

        public CompanyControllerTests()
        {
            _userServicesMock = new Mock<IUserServices>();

            _controller = new CompanyController(_userServicesMock.Object);
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
                NIP = "0123456789",
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

            var userId = 1;

            var result = await _controller.AddCompany(userId, company);

            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            badRequestResult.Value.Should().Be("NIP must be a 10 digit number.");
        }



    }
}
