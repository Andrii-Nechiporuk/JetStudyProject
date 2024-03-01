using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace JetStudyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Endpoint goal is to add Instructor role to user who asks
        /// </summary>
        /// <returns></returns>
        [HttpPost("become-instructor")]
        public async Task<IActionResult> BecomeInstructor()
        {
            var userId = await _userService.GetUserId(User);
            await _userService.BecomeInstructor(userId);
            return Ok();
        }

        /// <summary>
        /// Changes the user's username
        /// </summary>
        [HttpPut("change-username")]
        [Authorize]
        public async Task<IActionResult> ChangeUserName(string name)
        {
            await _userService.ChangeUserName(User, name);
            return Ok("Username changed successfully");
        }

        /// <summary>
        /// Changes the user's name
        /// </summary>
        [HttpPut("change-name")]
        [Authorize]
        public async Task<IActionResult> ChangeName(string name)
        {
            await _userService.ChangeName(User, name);
            return Ok("Name changed successfully");
        }

        /// <summary>
        /// Changes the user's surname
        /// </summary>
        [HttpPut("change-surname")]
        [Authorize]
        public async Task<IActionResult> ChangeSurname(string name)
        {
            await _userService.ChangeSurname(User, name);
            return Ok("Surname changed successfully");
        }

        /// <summary>
        /// Changes the user's password
        /// </summary>
        [HttpPut("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(string currentPassword, string newPassword)
        {
            await _userService.ChangePassword(User, currentPassword, newPassword);
            return Ok("Password changed successfully");
        }

        /// <summary>
        /// Changes the user's email
        /// </summary>
        [HttpPut("change-email")]
        [Authorize]
        public async Task<IActionResult> ChangeEmail(string currentPassword, string newEmail)
        {
            await _userService.ChangeEmail(User, currentPassword, newEmail);
            return Ok("Email changed successfully");
        }
    }
}
