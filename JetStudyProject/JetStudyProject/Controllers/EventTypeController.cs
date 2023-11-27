using JetStudyProject.Infrastracture.DTOs.CategoryDTOs;
using JetStudyProject.Infrastracture.DTOs.EventTypeDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace JetStudyProject.Controllers
{
    /// <summary>
    /// Controller to manipulate types of events of event
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class EventTypeController : ControllerBase
    {
        private readonly IEventTypeService _eventTypeService;

        public EventTypeController(IEventTypeService eventTypeService)
        {
            _eventTypeService = eventTypeService;
        }

        /// <summary>
        /// Returns the list of existing types of event
        /// </summary>
        [HttpGet]
        public List<EventTypeDto> GetPreviewEventTypes()
        {
            return _eventTypeService.GetEventTypes();
        }

        /// <summary>
        /// Creates event types from a list of types separated by commas
        /// </summary>
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> MultipleCreateEventTypes([FromBody] List<string> eventTypesNames)
        {
            await _eventTypeService.MultipleCreateEventTypes(eventTypesNames);
            return Ok();
        }

        /// <summary>
        /// Deletes type of event from the database
        /// </summary>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteEventType(int id)
        {
            await _eventTypeService.DeleteEventType(id);
            return Ok();
        }
    }
}
