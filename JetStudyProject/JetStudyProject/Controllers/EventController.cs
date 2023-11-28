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

        [HttpGet("authorized/{id}")]
        [Authorize]
        public async Task<EventFullDto>? GetEventAuthorized(int id)
        {
            var userId = await _userService.GetUserId(User);
            return await _eventService.GetEventAuthorized(id, userId);
        }

        [HttpGet("{id}")]
        public async Task<EventFullDto>? GetEvent(int id)
        {
            return await _eventService.GetEvent(id);
        }

        [HttpGet]
        public List<EventPreviewDto> GetEventsPreviews()
        {
            return  _eventService.GetEventsPreviews();
        }

        /// <summary>
        /// Returns a list of events filtered based on the provided parameters
        /// </summary>
        /// <param name="searchString">The string to search for in the post title</param>
        /// <param name="dateFilter">The filter to apply to the post date. Valid values are "Week", "Month" and "Year"</param>
        /// <param name="categoryId">The field to sort the posts by categories. Set to 0 for default.</param>
        /// <param name="eventTypeId">The field to sort the posts by event types. Set to 0 for default.</param>
        [HttpGet("parameters")]
        public List<EventPreviewDto> GetPreviewEventsWithFilter(string? searchString, string? dateFilter, int categoryId, int eventTypeId)
        {
            return _eventService.GetSortedFilteredEventPreviews(searchString, dateFilter, categoryId, eventTypeId);
        }
        /// <summary>
        /// Sends request to event and user recives email with card credentials of instructor
        /// </summary>
        /// <param name="eventId">Id of event which student wants to purchase</param>
        /// <returns></returns>
        [HttpPost("send-request-to-event")]
        [Authorize]
        public async Task<IActionResult> SendRequest(int eventId)
        {
            var userId = await _userService.GetUserId(User);
            await _eventService.SendRequest(userId, eventId);
            return Ok();
        }
    }
}
