using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IEventService
    {
        List<EventPreviewDto> GetEventsPreviews();
        Task<EventFullDto> GetEventAuthorized(int id, string userId);
        Task<EventFullDto> GetEvent(int id);
        List<EventPreviewDto> GetSortedFilteredEventPreviews(string? searchString, string? dateFilter, int categoryId, int eventTypeId);
        List<EventPreviewDto> GetThisWeekEventPreviews();
        List<EventPreviewDto> GetThisMonthEventPreviews();
        List<EventPreviewDto> GetEventPreviewsAfterThisMonths();
        Task CreateEvent(EventCreateDto eventCreateDto, string userId);
        Task EditEvent(EventEditDto eventEditDto, string userId, int eventId);
        Task DeleteEvent(int eventId, string userId);
        Task SendEventToModerate(int eventId, string userId);
        Task ApproveEvent(int eventId, string userId);
        Task SendRequest(string userId, int eventId);
    }
}
