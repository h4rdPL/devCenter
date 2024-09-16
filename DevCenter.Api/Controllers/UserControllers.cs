using DevCenter.Api.Dto;
using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;
        private readonly IConfiguration _configuration;
        public UserController(UserServices userServices, IConfiguration configuration)
        {
            _userServices = userServices;
            _configuration = configuration;
        }


        /// <summary>
        /// Registers a new user in the system.
        /// </summary>
        /// <param name="user">The user details for registration.</param>
        /// <returns>
        /// A 201 Created response with the user's email if the registration is successful;
        /// a 409 Conflict response if the user already exists;
        /// or a 400 Bad Request response if registration fails.
        /// </returns>
        [HttpPost("register")]
        public async Task<ActionResult<User>> UserRegister([FromBody] UserDTO user)
        {
            try
            {
                var result = await _userServices.RegisterUser(
                    user.Username,
                    user.Email,
                    user.Password,
                    user.Role
                );

                if (result.Success)
                {
                    return CreatedAtAction(nameof(UserRegister), new { email = user.Email }, result.Data);
                }
                else
                {
                    return result.Message.Contains("already exists") ? Conflict(result.Message) : BadRequest(result.Message);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }


        [HttpPost("auth/google")]
        public async Task<IActionResult> AuthenticateGoogle([FromBody] GoogleLoginDTO googleLoginDTO)
        {
            var token = googleLoginDTO.Token;

            if (string.IsNullOrEmpty(token))
            {
                return BadRequest("Token is required.");
            }

            try
            {
                var user = await _userServices.AuthenticateGoogleUser(token);

                if (user != null)
                {
                    return Ok(user); 
                }
                else
                {
                    return Unauthorized("Invalid Google token or user not saved.");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpGet("counter")]
        public async Task<IActionResult> GetCounter()
        {
            try
            {
                var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

                if (string.IsNullOrEmpty(emailClaim))
                {
                    return BadRequest("User email not found.");
                }

                Console.WriteLine($"Fetching counter for user with email: {emailClaim}");

                var counter = await _userServices.GetCounterByEmail(emailClaim);
                return Ok(counter);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [Authorize]
        [HttpPost("counter")]
        public async Task<IActionResult> UpdateCounter([FromBody] int incrementValue)
        {
            var emailClaim = User.FindFirst(ClaimTypes.Email)?.Value;

            if (string.IsNullOrEmpty(emailClaim))
            {
                return Unauthorized("Email claim not found.");
            }

            var currentCounter = await _userServices.GetCounterByEmail(emailClaim);

            var newCounterValue = currentCounter + incrementValue;

            var success = await _userServices.UpdateCounter(emailClaim, newCounterValue);

            return success ? Ok() : StatusCode(500, "Error updating counter");
        }





    }

    public class GoogleLoginDTO
    {
        public string Token { get; set; }
    }
}
