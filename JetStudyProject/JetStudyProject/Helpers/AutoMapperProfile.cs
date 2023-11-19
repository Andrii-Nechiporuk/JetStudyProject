using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.DTOs.UserDTOs;

namespace JetStudyProject.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, from => from.MapFrom(x => x.Name + " " + x.Surname));
            CreateMap<ReadCourse, UserDto>()
                .ForMember(dest => dest.FullName, from => from.MapFrom(x => x.User.Name + " " + x.User.Surname))
                .ForMember(dest => dest.Id, from => from.MapFrom(x => x.UserId));

            CreateMap<Event, EventFullDto>()
                .ForMember(dest => dest.EventType, from => from.MapFrom(x => x.EventType.Title))
                .ForMember(dest => dest.Lectorers, from => from.MapFrom(x => x.Lecturers.ToList()));
        }
    }
}
