﻿using AutoMapper;
using JetStudyProject.Core.Specifications;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public EventService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<EventFullDto> GetPost(int id)
        {
            if (IsExist(id))
            {
                var eventFromDb = _unitOfWork.EventRepository.GetFirstBySpec(new Events.ByEventIdWithUserAndEventAndLectorers(id));
                var eventToSend = _mapper.Map<EventFullDto>(eventFromDb);
                var applicationToEvent = _unitOfWork.ApplicationToEventRepository.GetAll();
                return eventToSend;
            }
            else return new EventFullDto();
        }

        public List<EventPreviewDto> GetPostPreview()
        {
            throw new NotImplementedException();
        }

        public bool IsExist(int id)
        {
            if (id <= 0) return false;

            var post = _unitOfWork.EventRepository.GetById(id);

            if (post == null) return false;
            return true;
        }
    }
}
