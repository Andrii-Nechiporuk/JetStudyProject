﻿using AutoMapper;
using JetStudyProject.Core.Entities;
using JetStudyProject.Infrastracture.DTOs.BasketDTOs;
using JetStudyProject.Infrastracture.DTOs.CategoryDTOs;
using JetStudyProject.Infrastracture.DTOs.EventDTOs;
using JetStudyProject.Infrastracture.DTOs.EventTypeDTOs;
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

            CreateMap<EventFullDto, Event>();
            CreateMap<EventCreateDto, Event>();
            CreateMap<EventEditDto, Event>()
            .ForAllMembers(opts =>
            {
                opts.AllowNull();
                opts.Condition((src, dest, srcMember) => srcMember != null);
            });


            CreateMap<Event, EventPreviewDto>()
                .ForMember(dest => dest.EventType, from => from.MapFrom(x => x.EventType.Title))
                .ForMember(dest => dest.Lecturers, from => from.MapFrom(x => x.Lecturers.ToList()))
                .ForMember(dest => dest.ImageSrc, from => from.MapFrom(x => Path.Combine(_server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault(), WebConstants.eventsImagesPath, x.Thumbnail)));

            CreateMap<Category, CategoryDto>();
            CreateMap<CategoryDto, Category>();

            CreateMap<EventType, EventTypeDto>();
            CreateMap<EventTypeDto, EventType>();

            CreateMap<BasketItem, BasketItemDto>()
                .ForMember(dest => dest.Title, from => from.MapFrom(x=> x.Event.Title))
                .ForMember(dest => dest.Price, from => from.MapFrom(x => x.Event.Price))
                .ForMember(dest => dest.ImageSrc, from => from.MapFrom(x => Path.Combine(_server.Features.Get<IServerAddressesFeature>().Addresses.FirstOrDefault(), WebConstants.eventsImagesPath, x.Event.Thumbnail)));
            CreateMap<BasketItemDto, BasketItem>();

            CreateMap<Basket, BasketDto>()
                .ForMember(dest => dest.Items, from => from.MapFrom(x => x.BasketItems.ToList()));

        }
    }
}
