using JetStudyProject.Infrastracture.Interfaces;
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
    }
}
