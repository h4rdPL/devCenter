using DevCenter.Api.Dto;
using DevCenter.Application.Users;
using DevCenter.Domain.Entieties;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Http.Connections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Json;

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


    }

}
