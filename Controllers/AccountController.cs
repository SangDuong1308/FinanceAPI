using api.Dtos.Account;
using api.Interfaces;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace api.Controllers
{
    [Route("api/account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly SignInManager<AppUser> _signinManager;
        private readonly ILogger<AccountController> _logger;

        // Ensure there is only one constructor
        public AccountController(UserManager<AppUser> userManager, ITokenService tokenService, SignInManager<AppUser> signinManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _tokenService = tokenService;
            _signinManager = signinManager;
            _logger = logger;
        }


        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var appUser = new AppUser
                {
                    UserName = registerDto.Username,
                    Email = registerDto.Email,
                };

                var createUser = await _userManager.CreateAsync(appUser, registerDto.Password);
                if (createUser.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(appUser, "User");
                    if (roleResult.Succeeded)
                    {
                        return Ok(new NewUserDto
                        {
                            UserName = appUser.UserName,
                            Email = appUser.Email,
                        });
                    }
                    else
                    {
                        return StatusCode(500, roleResult.Errors);
                    }
                }
                else
                {
                    return StatusCode(500, createUser.Errors);
                }
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.Username.ToLower());
                if (user == null) return Unauthorized("Invalid Username!");

                var result = await _signinManager.CheckPasswordSignInAsync(user, loginDto.Password, false);

                if (!result.Succeeded) return Unauthorized("Username not found and/or password incorrect");

                JwtSecurityToken token = _tokenService.CreateToken(user);

                var refreshToken = _tokenService.GenerateRefreshToken();

                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiry = DateTime.UtcNow.AddMinutes(5);

                await _userManager.UpdateAsync(user);

                //return Ok(
                //   new NewUserDto
                //   {
                //       UserName = user.UserName,
                //       Email = user.Email,
                //       Token = _tokenService.CreateToken(user)
                //   }
                //);

                return Ok(new LoginResponseDto
                {
                    Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                    Expiration = token.ValidTo,
                    RefreshToken = refreshToken
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("Refresh")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Refresh([FromBody] RefreshDto refreshDto)
        {
            _logger.LogInformation("Refresh called");

            var principle = _tokenService.GetPrincipalFromExpiredToken(refreshDto.AccessToken);
            if (principle?.Identity?.Name is null)
                return Unauthorized();

            var user = await _userManager.FindByNameAsync(principle.Identity.Name);

            //if (user == null)
            //{
            //    return Unauthorized("User not exist");
            //}

            if (user is null || user.RefreshToken != refreshDto.RefreshToken || user.RefreshTokenExpiry < DateTime.UtcNow)
            {
                return Unauthorized("User not found or refresh token expired.");
            }

            var token = _tokenService.CreateToken(user);
            _logger.LogInformation("Refresh succeeded");

            return Ok(new LoginResponseDto
            {
                Jwt = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo,
                RefreshToken = refreshDto.RefreshToken
            });
        }
    }
}
