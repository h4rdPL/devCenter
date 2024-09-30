using DevCenter.Api.Dto;
using DevCenter.Application.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DevCenter.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserServices _userServices;
    private readonly IConfiguration _configuration;
    private readonly IUserClaimsService _userClaimsService;
    public UserController(IUserServices userServices, IConfiguration configuration, IUserClaimsService userClaimsService)
    {
        _userServices = userServices;
        _configuration = configuration;
        _userClaimsService = userClaimsService;
    }


    [HttpPost("register")]
    public async Task<ActionResult<UserDTO>> UserRegister([FromBody] UserDTO user)
    {
        try
        {
            var result = await _userServices.RegisterUser(user.Username, user.Email, user.Password, user.Role);

            if (result.IsSuccess)
            {
                return CreatedAtAction(nameof(UserRegister), new { email = user.Email }, result.Value);
            }
            else
            {
                return result.ErrorMessage.Contains("already exists") ? Conflict(result.ErrorMessage) : BadRequest(result.ErrorMessage);
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
            var result = await _userServices.AuthenticateGoogleUser(token);

            if (result.IsSuccess)
            {
                return Ok(result.Value);
            }
            else
            {
                return Unauthorized(result.ErrorMessage);
            }
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

 

}
