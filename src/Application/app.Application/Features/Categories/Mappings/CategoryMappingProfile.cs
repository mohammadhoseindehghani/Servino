using app.Application.Contracts.DTOs.CategoryDTOs;
using app.Domain.CategoryAgg.Entities;
using AutoMapper;

namespace app.Application.Features.Categories.Mappings;

public class CategoryMappingProfile : Profile
{
    public CategoryMappingProfile()
    {
        CreateMap<Category, BreadcrumbDto>();

        CreateMap<Category, CategoryClientDto>()
            .ForMember(dest => dest.HasChildren,
                opt => opt.MapFrom(src => src.SubCategories
                    .Any(sc => !sc.IsDeleted && sc.IsActive)));

        CreateMap<Category, CategoryDto>();





    }
}