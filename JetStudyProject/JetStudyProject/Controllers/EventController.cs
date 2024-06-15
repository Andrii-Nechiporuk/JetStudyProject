using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.EmailDTOs;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
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
        public EventController(IEventService eventService, IUserService userService)
        {
            _eventService = eventService;
            _userService = userService;
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

        /// <summary>
        /// Returns quantity of pages of events in DB
        /// </summary>
        /// <param name="pageSize">The quantity of events to calculate pages quantity</param>
        /// <returns>Returns quantity of pages of events in DB</returns>
        [HttpGet("get-pages-quantity")]
        public PagesCountDTO GetPagesQuantity(int pageSize = 6)
        {
            return _eventService.GetPagesQuantity(pageSize);
        }


        /// <summary>
        /// Returns a list of events from DB
        /// </summary>
        /// <param name="page">The page to count from when taking events from DB. Set to 1 for default.</param>
        /// <param name="pageSize">The quantity of events to take from DB. Set to 6 for default.</param>
        [HttpGet]
        public List<EventPreviewDto> GetEventsPreviews(int page = 1, int pageSize = 6)
        {
            return  _eventService.GetEventsPreviews(page, pageSize);
        }

        /// <summary>
        /// Returns a list of events filtered based on the provided parameters
        /// </summary>
        /// <param name="search">The string to search for in the post title</param>
        /// <param name="dateFilter">The filter to apply to the post date. Valid values are "Week", "Month" and "Year"</param>
        /// <param name="categoryId">The field to sort the posts by categories. Set to 0 for default.</param>
        /// <param name="eventTypeId">The field to sort the posts by event types. Set to 0 for default.</param>
        /// <param name="page">The page to count from when taking events from DB. Set to 1 for default.</param>
        /// <param name="pageSize">The quantity of events to take from DB. Set to 6 for default.</param>
        [HttpGet("parameters")]
        public List<EventPreviewDto> GetPreviewEventsWithFilter(string? search, string? dateFilter, int categoryId, int eventTypeId, int page = 1, int pageSize = 6)
        {
            return _eventService.GetSortedFilteredEventPreviews(search, page, pageSize, dateFilter, categoryId, eventTypeId);
        }

        /// <summary>
        /// Returns list with events for this week
        /// </summary>
        /// <param name="page">The page to count from when taking events from DB. Set to 1 for default.</param>
        /// <param name="pageSize">The quantity of events to take from DB. Set to 6 for default.</param>
        [HttpGet("get-this-week-events")]
        public List<EventPreviewDto> GetThisWeekEvents(int page = 1, int pageSize = 6)
        {
            return _eventService.GetThisWeekEventPreviews(page, pageSize);
        }

        /// <summary>
        /// Returns list with events for this month
        /// </summary>
        /// <param name="page">The page to count from when taking events from DB. Set to 1 for default.</param>
        /// <param name="pageSize">The quantity of events to take from DB. Set to 6 for default.</param>
        [HttpGet("get-this-month-events")]
        public List<EventPreviewDto> GetThisMonthEvents(int page = 1, int pageSize = 6)
        {
            return _eventService.GetThisMonthEventPreviews(page, pageSize);
        }

        /// <summary>
        /// Returns list with events after this month
        /// </summary>
        /// <param name="page">The page to count from when taking events from DB. Set to 1 for default.</param>
        /// <param name="pageSize">The quantity of events to take from DB. Set to 6 for default.</param>
        [HttpGet("get-after-month-events")]
        public List<EventPreviewDto> GetAfterMonthEvents(int page = 1, int pageSize = 6)
        {
            return _eventService.GetEventPreviewsAfterThisMonths(page, pageSize);
        }

        /// <summary>
        /// Creates event in db and transfers image to root folder
        /// </summary>
        /// <param name="eventCreateDto">StartDate format [YYYY-MM-DD]; StartTime format [HH:MM:SS]</param>
        /// <returns></returns>
        [HttpPost("create")]
        [Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> CreateEvent([FromForm] EventCreateDto eventCreateDto)
        {
            var userId = await _userService.GetUserId(User);
            await _eventService.CreateEvent(eventCreateDto, userId);
            return Ok();
        }

        /// <summary>
        /// Edits a post by ID
        /// </summary>
        /// <param name="id">The ID of the post to edit</param>
        [HttpPut("{id}")]
        [Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> EditPost(int id, [FromForm] EventEditDto eventCteateDto)
        {
            var userId = await _userService.GetUserId(User);
            await _eventService.EditEvent(eventCteateDto, userId, id);
            return Ok();
        }

        /// <summary>
        /// Deletes an event by ID
        /// </summary>
        /// <param name="id">The ID of the event to delete</param>
        [HttpDelete("common-delete")]
        [Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var userId = await _userService.GetUserId(User);

            await _eventService.DeleteEvent(id, userId);

            return Ok("Подію успішно видалено");
        }

        /// <summary>
        /// Sends event to moderation by ID
        /// </summary>
        /// <param name="id">The ID of the event</param>
        [HttpPut("send-event-to-moderation")]
        [Authorize(Roles = "Instructor, Admin")]
        public async Task<IActionResult> SendEventToModeration(int id)
        {
            var userId = await _userService.GetUserId(User);

            await _eventService.SendEventToModerate(id, userId);

            return Ok("Подію успішно надіслано на модерацію");
        }

        /// <summary>
        /// Approves event by ID, only for admins
        /// </summary>
        /// <param name="id">The ID of the event</param>
        [HttpPut("approve-event")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ApproveEvent(int id)
        {
            var userId = await _userService.GetUserId(User);

            await _eventService.ApproveEvent(id, userId);

            return Ok("Подію успішно опубліковано");
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
