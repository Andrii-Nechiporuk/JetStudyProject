﻿using JetStudyProject.Infrastracture.DataAccess;
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
        /// Returns list with events for this week
        /// </summary>
        [HttpGet("get-this-week-events")]
        public List<EventPreviewDto> GetThisWeekEvents()
        {
            return _eventService.GetThisWeekEventPreviews();
        }

        /// <summary>
        /// Returns list with events for this month
        /// </summary>
        [HttpGet("get-this-month-events")]
        public List<EventPreviewDto> GetThisMonthEvents()
        {
            return _eventService.GetThisMonthEventPreviews();
        }

        /// <summary>
        /// Returns list with events after this month
        /// </summary>
        [HttpGet("get-after-month-events")]
        public List<EventPreviewDto> GetAfterMonthEvents()
        {
            return _eventService.GetEventPreviewsAfterThisMonths();
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
