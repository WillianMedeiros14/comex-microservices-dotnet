using AutoMapper;
using OrderService.DTOs.Order;
using OrderService.Models;

namespace OrderService.Profiles
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<OrderCreateDTO, Order>();
            CreateMap<OrderItemCreateDTO, OrderItem>();
            CreateMap<Order, OrderReadDTO>();
            CreateMap<OrderItem, OrderItemReadDTO>();
        }
    }
}
