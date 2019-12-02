using AutoMapper;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDTO, User>()
                .ForMember(
        dest => dest.Age,
        opt => opt.MapFrom(src => src.Age == null ? (int?)null : int.Parse(src.Age))
    );

            CreateMap<ProductDTO, Product>()
                   .ForMember(
        dest => dest.SellerId,
        opt => opt.MapFrom(src => 1));

        }
    }
}
