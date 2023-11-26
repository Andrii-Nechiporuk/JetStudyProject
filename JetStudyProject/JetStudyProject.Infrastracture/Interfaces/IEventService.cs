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
    }
}
