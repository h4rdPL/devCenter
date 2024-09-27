using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUserServices _userServices;
        public CompanyController(IUserServices userServices)
        {
            _userServices = userServices;
        }

        [Authorize]
        [HttpPost("{userId}/company")]
        public async Task<IActionResult> AddCompany(int userId, [FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest("Company is null");
            }

            if (string.IsNullOrWhiteSpace(company.NIP) ||
                string.IsNullOrWhiteSpace(company.Name) ||
                string.IsNullOrWhiteSpace(company.Country) ||
                string.IsNullOrWhiteSpace(company.City) ||
                string.IsNullOrWhiteSpace(company.PostalCode) ||
                string.IsNullOrWhiteSpace(company.Street) ||
                string.IsNullOrWhiteSpace(company.CompanyEmail))
            {
                return BadRequest("Company details cannot be empty.");
            }
            if (company.NIP.Length != 10)
            {
                return BadRequest("NIP must be a 10 digit number.");
            }


            var result = await _userServices.AddCompanyToUser(userId, company);

            if (result.IsSuccess)
            {
                return Ok();
            }

            return BadRequest(result.ErrorMessage);
        }
    }
}
