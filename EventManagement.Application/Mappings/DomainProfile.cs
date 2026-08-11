using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using EventManagement.Application.DTOs.Category.Query;
using EventManagement.Domain.Entities;

namespace EventManagement.Application.Mappings
{
    public class DomainProfile : Profile
    {
       public DomainProfile() 
        {
            CreateMap<Category, CategoryDto>().ReverseMap();
        }
    }
}
