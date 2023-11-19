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
        List<EventPreviewDto> GetPostPreview();
        Task<EventFullDto> GetPost(int id, string userId);
    }
}
