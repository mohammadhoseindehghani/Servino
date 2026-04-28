using app.Application.DTOs.CategoryDTOs;
using app.Domain.CategoryAgg.Entities;
using AutoMapper;

namespace app.Application.MappingProfiles;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Category, CategoryDto>();
    }
}