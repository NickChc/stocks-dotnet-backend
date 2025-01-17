using Api.Dtos.Account;
using Api.Interfaces;
using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

[Route("api/account")]
[ApiController]
public class AccountController(
    UserManager<AppUser> userManager,
    ITokenService tokenService,
    SignInManager<AppUser> signinManager
) : ControllerBase
{
    private readonly UserManager<AppUser> _userManager = userManager;
    private readonly ITokenService _tokenService = tokenService;

    private readonly SignInManager<AppUser> _signinManager = signinManager;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            AppUser appUser = new() { UserName = registerDto.Username, Email = registerDto.Email };

            var createdUser = await _userManager.CreateAsync(appUser, registerDto.Password!);

            if (!createdUser.Succeeded)
            {
                return StatusCode(500, createdUser.Errors);
            }

            var roleResult = await _userManager.AddToRoleAsync(appUser, "User");

            if (!roleResult.Succeeded)
            {
                return StatusCode(500, roleResult.Errors);
            }

            return Ok(
                new NewUserDto
                {
                    Username = appUser.UserName!,
                    Email = appUser.Email!,
                    Token = _tokenService.CreateToken(appUser),
                }
            );
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        AppUser? user = await _userManager.Users.FirstOrDefaultAsync(
            (u) => u.UserName == loginDto.Username
        );

        if (user is null)
        {
            return Unauthorized("Invalid credentials");
        }

        var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

        if (!result.Succeeded)
        {
            return Unauthorized("Invalid credentials");
        }

        return Ok(
            new NewUserDto
            {
                Email = user.Email!,
                Username = user.UserName!,
                Token = _tokenService.CreateToken(user),
            }
        );
    }
}
