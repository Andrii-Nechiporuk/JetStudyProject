using AutoMapper;
using Azure;
using JetStudyProject.Core.Entities;
using JetStudyProject.Core.Specifications;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using JetStudyProject.Infrastracture.Resources;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace JetStudyProject.Infrastracture.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
        }
        public async Task<EventFullDto> GetEventAuthorized(int id, string userId)
        {
            if (IsExist(id))
            {
                var eventFromDb = _unitOfWork.EventRepository.GetFirstBySpec(new Events.ByEventIdWithUserAndEventAndLectorers(id));
                var eventToSend = _mapper.Map<EventFullDto>(eventFromDb);

                var applicationToEvent = _unitOfWork.ApplicationToEventRepository.GetFirstBySpec(new ApplicationsToEvent.ByUserIdAndEventId(id, userId));
                if (applicationToEvent != null)
                    eventToSend.WaitingForConfirmation = true;
                else eventToSend.WaitingForConfirmation = false;

                var listenCourse = _unitOfWork.ListenCourseRepository.GetFirstBySpec(new ListenCourses.ByUserIdAndEventId(id, userId));
                if (listenCourse != null)
                    eventToSend.IsAllowedToWatchAllContent = true;
                else eventToSend.IsAllowedToWatchAllContent = false;

                return eventToSend;
            }
            else return new EventFullDto();
        }

        public async Task<EventFullDto> GetEvent(int id)
        {
            if (IsExist(id))
            {
                var eventFromDb = _unitOfWork.EventRepository.GetFirstBySpec(new Events.ByEventIdWithUserAndEventAndLectorers(id));
                var eventToSend = _mapper.Map<EventFullDto>(eventFromDb);

                return eventToSend;
            }
            else return new EventFullDto();
        }

        public List<EventPreviewDto> GetEventsPreviews()
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers());
            return _mapper.Map<List<EventPreviewDto>>(events);
        }

        public List<EventPreviewDto> GetSortedFilteredEventPreviews(string? searchString, string? dateFilter, int categoryId = 0, int eventTypeId = 0)
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers());

            if (searchString != null)
            {
                events = events.Where(p => p.Title.ToLower().Contains(searchString.ToLower()));
            }

            if (dateFilter != null)
            {
                switch (dateFilter)
                {
                    case "Week":
                        events = events.Where(p => p.StartDate <= DateTime.Now.AddDays(7) && p.StartDate >= DateTime.Now.Date);
                        break;
                    case "Months":
                        events = events.Where(p => p.StartDate <= DateTime.Now.AddMonths(1) && p.StartDate >= DateTime.Now.Date);
                        break;
                    case "Year":
                        events = events.Where(p => p.StartDate <= DateTime.Now.AddYears(1) && p.StartDate >= DateTime.Now.Date);
                        break;
                    default:
                        break;
                }
            }

            if (categoryId != 0)
            {
                events = events.Where(p => p.CategoryId == categoryId);
            }

            if (eventTypeId != 0)
            {
                events = events.Where(p => p.EventTypeId == eventTypeId);
            }

            return _mapper.Map<List<EventPreviewDto>>(events);
        }
        public async Task SendRequest(string userId, int eventId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var foundedEvent = _unitOfWork.EventRepository.GetById(eventId);
            var application = _unitOfWork.ApplicationToEventRepository.GetAll().FirstOrDefault(a => a.UserId == userId && a.EventId == eventId);
            if (user != null && foundedEvent != null && application == null)
            {
                await _unitOfWork.ApplicationToEventRepository.Insert(new ApplicationToEvent()
                {
                    EventId = eventId,
                    UserId = userId
                });
                await _unitOfWork.SaveAsync();
                await _emailService.SendEmail(user.Email, "Оплата курсу на платформі JetStudy", $"<h4>Ви подали заявку на курс {foundedEvent.Title}</h4>\r\n<hr />\r\n<p><strong></strong>Оплату потрібно здійснити на наступну карту <strong>4149 4324 9430 1230</strong> протягом 12 годин.</p>\r\n<p>Після цього вас буде зараховано на курс інструктором.</p>");
            }
            else if (application != null)
                throw new HttpException(ErrorMessages.ApplicationExist, HttpStatusCode.BadRequest);
            else if (user == null)
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            else if (foundedEvent == null)
                throw new HttpException(ErrorMessages.InvalidEventId, HttpStatusCode.BadRequest);
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
