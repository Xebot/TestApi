using AutoMapper;
using TestApi.Contracts.Models;
using TestApi.Domain.Entities;

namespace TestApi.Infrastructure.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>()
                .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.Id));

            CreateMap<Order, OrderDto>()
                .ForMember(dest => dest.OrderNumber, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.OrderItems, opt => opt.MapFrom(src => src.Items));

            CreateMap<OrderItem, OrderItemDto>()
                .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.Product.Id));

            CreateMap<Order, OrderShortInfoDto>()
                .ForSourceMember(x => x.Items, opt => opt.DoNotValidate())
                .ForSourceMember(x => x.Customer, opt => opt.DoNotValidate());
        }
    }
}
