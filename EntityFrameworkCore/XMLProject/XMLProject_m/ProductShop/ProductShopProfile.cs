using AutoMapper;
using ProductShop.DTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserDTO, User>();

            CreateMap<ProductDTO, Product>();

            CreateMap<CategoryDTO, Category>();

            CreateMap<CategoryProductDTO, CategoryProduct>();
        }
    }
}
