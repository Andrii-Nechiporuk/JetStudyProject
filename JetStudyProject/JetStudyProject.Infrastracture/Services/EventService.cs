using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Core.Specifications;
using JetStudyProject.Infrastracture.DataAccess;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.Exceptions;
using JetStudyProject.Infrastracture.Interfaces;
using JetStudyProject.Infrastracture.Resources;
using JetStudyProject.Infrastracture.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Azure;

namespace JetStudyProject.Infrastracture.Services
{
    public class EventService : IEventService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;
        private readonly IEmailService _emailService;

        public EventService(IUnitOfWork unitOfWork, IMapper mapper, UserManager<User> userManager, IEmailService emailService, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userManager = userManager;
            _emailService = emailService;
            _webHostEnvironment = webHostEnvironment;
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

        public PagesCountDTO GetPagesQuantity(int pageSize)
        {
            var totalCount = _unitOfWork.EventRepository.GetAll().Count();
            var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

            return new PagesCountDTO { PagesCount = totalPages };
        }

        public List<EventPreviewDto> GetEventsPreviews(int page, int pageSize)
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers(page, pageSize));
            events = events
                .Skip(((page - 1) * pageSize))
                .Take(pageSize);
            return _mapper.Map<List<EventPreviewDto>>(events);
        }

        public List<EventPreviewDto> GetSortedFilteredEventPreviews(string? searchString, int page, int pageSize, string? dateFilter, int categoryId = 0, int eventTypeId = 0)
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers(page, pageSize));

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

            events = events
                .Skip(((page - 1) * pageSize))
                .Take(pageSize);

            return _mapper.Map<List<EventPreviewDto>>(events);
        }

        public List<EventPreviewDto> GetThisWeekEventPreviews(int page, int pageSize)
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers(page, pageSize));

            events = events.Where(p => p.StartDate <= DateTime.Now.AddDays(7) && p.StartDate >= DateTime.Now.Date);

            events = events
                .Skip(((page - 1) * pageSize))
                .Take(pageSize);

            return _mapper.Map<List<EventPreviewDto>>(events);
        }

        public List<EventPreviewDto> GetThisMonthEventPreviews(int page, int pageSize)
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers(page, pageSize));

            var monthOfTheYear = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            events = events.Where(p => p.StartDate >= DateTime.Now.AddDays(7) &&
            p.StartDate <= DateTime.Now.AddDays(DateTime.DaysInMonth(year, monthOfTheYear) - DateTime.Now.Day));

            events = events
                .Skip(((page - 1) * pageSize))
                .Take(pageSize);

            return _mapper.Map<List<EventPreviewDto>>(events);
        }

        public List<EventPreviewDto> GetEventPreviewsAfterThisMonths(int page, int pageSize)
        {
            var events = _unitOfWork.EventRepository.GetListBySpec(new Events.WithUserAndEventAndLectorers(page, pageSize));

            var monthOfTheYear = DateTime.Now.Month;
            var year = DateTime.Now.Year;

            events = events.Where(p => p.StartDate >= DateTime.Now.AddDays(DateTime.DaysInMonth(year, monthOfTheYear) - DateTime.Now.Day));

            events = events
                .Skip(((page - 1) * pageSize))
                .Take(pageSize);

            return _mapper.Map<List<EventPreviewDto>>(events);
        }
        public async Task CreateEvent(EventCreateDto eventCreateDto, string userId)
        {
            var eventToCreate = _mapper.Map<Event>(eventCreateDto);

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            }

            eventToCreate.CreatorId = userId;
            if (eventCreateDto.ImageFile != null)
            {
                eventToCreate.Thumbnail = await SaveImage(eventCreateDto.ImageFile);
            }
            eventToCreate.StatusForInstructorId = 1;
            eventToCreate.StatusForAdministratorId = 1;
            eventToCreate.StatusForStudentId = 1;
            await _unitOfWork.EventRepository.Insert(eventToCreate);
            await _unitOfWork.SaveAsync();
        }

        public async Task EditEvent(EventEditDto eventEditDto, string userId, int eventId)
        {
            if (!IsExist(eventId))
            {
                throw new HttpException(ErrorMessages.EventDoesNotExist, HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            }

            var eventToEdit = _unitOfWork.EventRepository.GetById(eventId);

            if (eventToEdit.CreatorId != userId && !await _userManager.IsInRoleAsync(user, "Instructor"))
            {
                throw new HttpException(ErrorMessages.InvalidPermission, HttpStatusCode.BadRequest);
            }

            if (eventEditDto.ImageFile != null)
            {
                DeleteImage(eventToEdit.Thumbnail);
                eventToEdit.Thumbnail = await SaveImage(eventEditDto.ImageFile);
            }

            _mapper.Map<EventEditDto, Event>(eventEditDto, eventToEdit);

            _unitOfWork.EventRepository.Update(eventToEdit);
            await _unitOfWork.SaveAsync();
        }

        public async Task DeleteEvent(int eventId, string userId)
        {
            if (!IsExist(eventId))
            {
                throw new HttpException(ErrorMessages.EventDoesNotExist, HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            }

            var eventDelete = _unitOfWork.EventRepository.GetById(eventId);

            if (eventDelete.CreatorId != user.Id)
            {
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            }

            if (!(eventDelete.StatusForInstructorId == 1 || eventDelete.StatusForInstructorId == 2))
            {
                throw new HttpException("Неможливо видалити опубліковану подію, зв'яжіться з адміністратором", HttpStatusCode.BadRequest);
            }

            DeleteImage(eventDelete.Thumbnail);
            _unitOfWork.EventRepository.Delete(eventId);
            await _unitOfWork.SaveAsync();
        }

        public async Task SendEventToModerate(int eventId, string userId)
        {
            if (!IsExist(eventId))
            {
                throw new HttpException(ErrorMessages.EventDoesNotExist, HttpStatusCode.BadRequest);
            }

            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            }

            var eventToEdit = _unitOfWork.EventRepository.GetById(eventId);

            if (eventToEdit.CreatorId != user.Id)
            {
                throw new HttpException("Ви не можете змінювати подію, яка вам не належить", HttpStatusCode.BadRequest);
            }

            if (!(eventToEdit.StatusForInstructorId == 1 || eventToEdit.StatusForInstructorId == 4))
            {
                throw new HttpException("Подія вже на модерації, або опублікована", HttpStatusCode.BadRequest);
            }

            eventToEdit.StatusForInstructorId = 2;
            _unitOfWork.EventRepository.Update(eventToEdit);
            await _unitOfWork.SaveAsync();
        }

        public async Task ApproveEvent(int eventId, string userId)
        {
            if (!IsExist(eventId))
            {
                throw new HttpException(ErrorMessages.EventDoesNotExist, HttpStatusCode.BadRequest);
            }

            var eventToEdit = _unitOfWork.EventRepository.GetById(eventId);

            if (eventToEdit.StatusForInstructorId == 3)
            {
                throw new HttpException("Подія вже опублікована", HttpStatusCode.BadRequest);
            }

            eventToEdit.StatusForInstructorId = 3;
            _unitOfWork.EventRepository.Update(eventToEdit);
            await _unitOfWork.SaveAsync();
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
                await _emailService.SendEmailAsync(user.Email, "Оплата курсу на платформі JetStudy", $"<h4>Ви подали заявку на курс {foundedEvent.Title}</h4>\r\n<hr />\r\n<p><strong></strong>Оплату потрібно здійснити на наступну карту <strong>4149 4324 9430 1230</strong> протягом 12 годин.</p>\r\n<p>Після цього вас буде зараховано на курс інструктором.</p>");
            }
            else if (application != null)
                throw new HttpException(ErrorMessages.ApplicationExist, HttpStatusCode.BadRequest);
            else if (user == null)
                throw new HttpException(ErrorMessages.InvalidUserId, HttpStatusCode.BadRequest);
            else if (foundedEvent == null)
                throw new HttpException(ErrorMessages.InvalidEventId, HttpStatusCode.BadRequest);
        }

        public async Task<string> SaveImage(IFormFile imageFile)
        {
            string imageName = new string(Path.GetFileNameWithoutExtension(imageFile.FileName).Take(10).ToArray()).Replace(' ', '-');
            imageName = imageName + DateTime.Now.ToString("yymmssfff") + Path.GetExtension(imageFile.FileName);
            var postFolder = Path.Combine(_webHostEnvironment.WebRootPath, WebConstants.eventsImagesPath);
            var imagePath = Path.Combine(postFolder, imageName);
            bool isExists = Directory.Exists(postFolder);

            if (!isExists)
            {
                Directory.CreateDirectory(postFolder);
            }

            using (var fileStream = new FileStream(imagePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(fileStream);
            }

            return imageName;
        }

        public void DeleteImage(string imageName)
        {
            var imagePath = Path.Combine(_webHostEnvironment.WebRootPath, WebConstants.eventsImagesPath, imageName);
            if (File.Exists(imagePath))
                File.Delete(imagePath);
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
