using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.DTOs.UserDTOs;
using JetStudyProject.Infrastracture.Utilities;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Hosting.Server.Features;

namespace JetStudyProject.Helpers
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile(IServer _server)
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.FullName, from => from.MapFrom(x => x.Name + " " + x.Surname));
            CreateMap<ReadCourse, UserDto>()
                .ForMember(dest => dest.FullName, from => from.MapFrom(x => x.User.Name + " " + x.User.Surname))
                .ForMember(dest => dest.Id, from => from.MapFrom(x => x.UserId));

            CreateMap<Event, EventFullDto>()
                .ForMember(dest => dest.EventType, from => from.MapFrom(x => x.EventType.Title))
                .ForMember(dest => dest.Lecturers, from => from.MapFrom(x => x.Lecturers.ToList()))
                .ForMember(dest => dest.ImageSrc, from => from.MapFrom(x => Path.Combine(_server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault(), WebConstants.eventsImagesPath, x.Thumbnail)));

            CreateMap<Event, EventPreviewDto>()
                .ForMember(dest => dest.EventType, from => from.MapFrom(x => x.EventType.Title))
                .ForMember(dest => dest.Lecturers, from => from.MapFrom(x => x.Lecturers.ToList()))
                .ForMember(dest => dest.ImageSrc, from => from.MapFrom(x => Path.Combine(_server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault(), WebConstants.eventsImagesPath, x.Thumbnail)));
        }
    }
}
