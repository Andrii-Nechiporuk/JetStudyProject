using JetStudyProject.Infrastracture.DTOs.EventTypeDTOs;

namespace JetStudyProject.Infrastracture.Interfaces
{
    public interface IEventTypeService
    {
        List<EventTypeDto> GetEventTypes();
        Task MultipleCreateEventTypes(List<string> evenTypeNames);
        Task DeleteEventType(int id);
        bool IsExist(int id);
    }
}
