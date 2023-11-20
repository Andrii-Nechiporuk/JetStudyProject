using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.EmailDTOs;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JetStudyProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IUserService _userService;
        private readonly IEmailService _emailService;

        public EventController(IEventService eventService, IUserService userService, IEmailService emailService)
        {
            _eventService = eventService;
            _userService = userService;
            _emailService = emailService;
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<EventFullDto>? GetPost(int id)
        {
            var userId = await _userService.GetUserId(User);
            return await _eventService.GetPost(id, userId);
        }

        [HttpPost("send-email")]
        public IActionResult SendEmail(EmailDto request)
        {
            _emailService.SendEmail(request);
            return Ok();
        }
    }
}
