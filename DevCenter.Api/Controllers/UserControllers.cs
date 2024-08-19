using DevCenter.Api.Dto;
using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DevCenter.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserServices _userServices;

        public UserController(UserServices userServices)
        {
            _userServices = userServices;
        }

        /// <summary>
        /// Initiates the Google OAuth2 login process.
        /// Redirects the user to the Google login page.
        /// </summary>
        /// <returns>A challenge result that redirects to Google for authentication.</returns>
        [HttpGet("login")]
        public IActionResult Login()
        {
            var props = new AuthenticationProperties { RedirectUri = "signin-google" };
            return Challenge(props, GoogleDefaults.AuthenticationScheme);
        }

        /// <summary>
        /// Handles the Google login callback.
        /// Processes the authentication response from Google and retrieves user information.
        /// </summary>
        /// <returns>
        /// A JSON response containing the user's name and email if authentication is successful;
        /// otherwise, returns a 400 Bad Request response.
        /// </returns>
        [HttpGet("signin-google")]
        public async Task<IActionResult> GoogleLogin()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            if (result?.Principal == null)
                return BadRequest("Failed to authenticate");

            var name = result.Principal.FindFirstValue(ClaimTypes.Name);
            var email = result.Principal.FindFirstValue(ClaimTypes.Email);

            return Ok(new { Name = name, Email = email });
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
    }
}
