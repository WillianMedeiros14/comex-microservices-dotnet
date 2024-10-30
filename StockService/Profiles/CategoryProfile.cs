using AutoMapper;
using StockService.Data.Dtos;
using StockService.Models;

namespace StockService.Profiles;
public class CategoryProfile : Profile
{
    public CategoryProfile()
    {
        CreateMap<CreateCategoryDto, Category>();
        CreateMap<UpdateCategoryDto, Category>();
        CreateMap<Category, UpdateCategoryDto>();
        CreateMap<Category, UpdateCategoryDto>();
    }
}