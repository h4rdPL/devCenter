using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using DevCenter.Domain.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly IUserServices _userServices;
        private readonly IUserClaimsService _userClaimsService;
        private readonly IUserRepository _userRepository;
        public CompanyController(IUserServices userServices, IUserClaimsService userClaimsService, IUserRepository userRepository)
        {
            _userServices = userServices;
            _userClaimsService = userClaimsService;
            _userRepository = userRepository;
        }

        [Authorize]
        [HttpPost("add")]
        public async Task<IActionResult> AddCompany([FromBody] Company company)
        {
            if (company == null)
            {
                return BadRequest("Company is null");
            }


            var emailClaim = await _userClaimsService.GetUserEmailClaimAsync(User);
            var user = await _userRepository.GetUserByEmail(emailClaim);

            var result = await _userServices.AddCompanyToUser(user.Id, company);

            if (result.IsSuccess)
            {
                return Ok("Company added successfully.");
            }

            return BadRequest(result.ErrorMessage);
        }

    }
}
