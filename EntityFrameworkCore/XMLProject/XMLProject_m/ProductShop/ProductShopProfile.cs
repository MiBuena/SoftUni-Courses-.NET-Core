using AutoMapper;
using ProductShop.DTOs;
using ProductShop.DTOs.ImportDTOs;
using ProductShop.Models;

namespace ProductShop
{
    public class ProductShopProfile : Profile
    {
        public ProductShopProfile()
        {
            CreateMap<UserImportDTO, User>();

            CreateMap<ProductImportDTO, Product>();

            CreateMap<CategoryImportDTO, Category>();

            CreateMap<CategoryProductImportDTO, CategoryProduct>();
        }
    }
}
