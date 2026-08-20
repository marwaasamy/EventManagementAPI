using AutoMapper;
using EventManagement.Application.DTOs.Category.Query;
using EventManagement.Application.DTOs.Event.Query;
using EventManagement.Application.DTOs.Venue.Command;
using EventManagement.Application.DTOs.Venue.Query;
using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.Mappings
{
    public class DomainProfile : Profile
    {
       public DomainProfile() 
        {
            CreateMap<Category, CategoryDto>().ReverseMap();

            CreateMap<Venue, VenueDTO>().ReverseMap();
            CreateMap<CreateVenueDTO, Venue>();
            CreateMap<UpdateVenueDTO, Venue>();

            CreateMap<Event, EventDto>()
            .ForMember(d => d.CategoryName, o => o.MapFrom(s => s.Category != null ? s.Category.Name : null))
            .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null));

            CreateMap<Event, EventListDto>()
                .ForMember(d => d.VenueName, o => o.MapFrom(s => s.Venue != null ? s.Venue.Name : null));
        }
    }
}
