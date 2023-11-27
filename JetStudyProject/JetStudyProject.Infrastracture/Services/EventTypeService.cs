using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.CategoryDTOs;
using JetStudyProject.Infrastracture.DTOs.EventTypeDTOs;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Services
{
    public class EventTypeService : IEventTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventTypeService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<EventTypeDto> GetEventTypes()
        {
            var eventTypes = _unitOfWork.EventTypeRepository.GetAll();
            var eventTypeDtos = _mapper.Map<List<EventTypeDto>>(eventTypes);
            return eventTypeDtos;
        }

        public async Task MultipleCreateEventTypes(List<string> eventTypeNames)
        {
            var eventTypeDtos = eventTypeNames.Select(x => new EventTypeDto{ Title = x.Trim() }).ToList();
            var existingEventTypes= GetEventTypes();
            var eventTypesToCreate = eventTypeDtos.Where(c => !existingEventTypes.Any(ec => ec.Title == c.Title)).ToList();
            if (eventTypesToCreate.Count > 0)
            {
                var eventTypes = _mapper.Map<List<EventType>>(eventTypesToCreate);
                foreach (var eventType in eventTypes)
                {
                    await _unitOfWork.EventTypeRepository.Insert(eventType);
                }
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new HttpException("All of the event types already exist in the database", HttpStatusCode.BadRequest);
            }
        }

        public async Task DeleteEventType(int eventTypeId)
        {
            if (IsExist(eventTypeId))
            {
                _unitOfWork.EventTypeRepository.Delete(eventTypeId);
                await _unitOfWork.SaveAsync();
            }
            else
            {
                throw new HttpException("Event type not found", HttpStatusCode.NotFound);
            }
        }

        public bool IsExist(int id)
        {
            if (id <= 0) return false;

            var tag = _unitOfWork.EventTypeRepository.GetById(id);

            if (tag == null) return false;
            return true;
        }
    }
}
