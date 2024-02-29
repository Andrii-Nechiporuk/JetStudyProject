using JetStudyProject.Infrastracture.DTOs.UserDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Reflection.Emit;

namespace JetStudyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            await _authService.Register(userRegisterDto);

            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserLoginDto userLoginDto)
        {
            var token = await _authService.Login(userLoginDto);
            return Ok(token);
        }

        [HttpPost("login-with-google")]
        public async Task<IActionResult> LoginWithGoogle([FromBody] string credential)
        {
            var token = await _authService.LoginWithGoogle(credential);
            return Ok(token);
        }

        [HttpGet("check-authorize")]
        [Authorize(Roles = "Student")]
        public IActionResult CheckAuthorize()
        {
            return Ok();
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _authService.Logout();
            return Ok();
        }

        [HttpGet("confirm-email")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            await _authService.ConfirmEmail(userId, code);
            return Ok("Електронну адресу підтверджено успішно");
        }
    }
}
