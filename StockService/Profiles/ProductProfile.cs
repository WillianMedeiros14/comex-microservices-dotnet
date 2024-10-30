using AutoMapper;
using StockService.Data.Dtos;
using StockService.Models;

namespace StockService.Profiles;
public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<CreateProductDto, Product>();
        CreateMap<UpdateProductDto, Product>();
        CreateMap<Product, UpdateProductDto>();
        CreateMap<Product, ReadProductDto>();
        CreateMap<Product, ProductResponseDto>();
    }
}