using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using NZWalks.Models.DTO;
using NZWalks.Repositories;

namespace NZWalks.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<IdentityUser> userManager;
    private readonly ITokenRepository tokenRepository;

    public AuthController(UserManager<IdentityUser> userManager, ITokenRepository tokenRepository)
    {
        this.userManager = userManager;
        this.tokenRepository = tokenRepository;
    }


    // POST: /api/Auth/Register
    [HttpPost]
    [Route("Register")]

    public async Task<IActionResult> Register([FromBody] RegisterRequestDto registerRequestDto)
    {
        var identityUser = new IdentityUser
        {
            UserName = registerRequestDto.UserName,
            Email = registerRequestDto.UserName,
        };

        var IdentityResult = await userManager
            .CreateAsync(identityUser, registerRequestDto.Password);

        if (IdentityResult.Succeeded)
        {
            // Add roles to this User
            if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
            {
                IdentityResult = await userManager
                    .AddToRolesAsync(identityUser, registerRequestDto.Roles);

                if (IdentityResult.Succeeded)
                {
                    return Ok("User was registered, Please login.");
                }
            }
        }
        return BadRequest("Something went wrong");
    }
    // POST: /api/Auth/Login
    [HttpPost]
    [Route("Login")]

    public async Task<IActionResult> Login([FromBody] LoginRequestDto loginRequestDto)
    {
        var user = await userManager
            .FindByEmailAsync(loginRequestDto.UserName);
        if (user != null)
        {
            var checkPasswordResult = await userManager.CheckPasswordAsync(user, loginRequestDto.Password);
            if (checkPasswordResult)
            {
                // Get roles for this user
                var roles = await userManager.GetRolesAsync(user);

                // Create Token
                if (roles != null)
                {
                    var jwtToken = tokenRepository.CreateJwtToken(user, roles.ToList());
                    var response = new LoginResponseDto
                    {
                        JwtToken = jwtToken
                    };
                    return Ok(jwtToken);
                }
            }
        }

        return BadRequest("Username or Password Incorrect");
    }
}