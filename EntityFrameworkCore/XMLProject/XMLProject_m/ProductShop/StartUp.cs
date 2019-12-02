using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.DTOs.ExportDTOs;
using ProductShop.DTOs.ImportDTOs;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                string xmlString = System.IO.File.ReadAllText("./../../../Datasets/categories-products.xml");

                //context.Database.Migrate();
                var result = GetSoldProducts(context);

                Console.WriteLine(result);
            }
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var usersWithSoldItem = context.Users
                .Where(x => x.ProductsSold.Any(y => y.BuyerId != null))
                .OrderBy(x=>x.LastName)
                .ThenBy(x=>x.FirstName)
                .Take(5)
                .Select(x=>
                new UsersWithSoldProductsExportDTO()
                { 
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(y => new SoldProductsExportDTO()
                    {
                        Name = y.Name,
                        Price = y.Price
                    }).ToList()
                })
                .ToList();


            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<UsersWithSoldProductsExportDTO>),
                new XmlRootAttribute("Users"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, usersWithSoldItem, namespaces);
            }

            return sb.ToString();
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new ProductsInRangeExportDTO()
                {
                    Name = x.Name,
                    Price = x.Price,
                    Buyer = $"{x.Buyer.FirstName} {x.Buyer.LastName}"
                })
                .Take(10)
                .ToList();

            var sb = new StringBuilder();

            var serializer =
                new XmlSerializer(typeof(List<ProductsInRangeExportDTO>),
                new XmlRootAttribute("Products"));

            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("", "");

            using (var writer = new StringWriter(sb))
            {
                serializer.Serialize(writer, products, namespaces);
            }

            return sb.ToString();
        }


        public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryProductImportDTO>), new XmlRootAttribute("CategoryProducts"));

            var deserializedCategories = new List<CategoryProductImportDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedCategories = (List<CategoryProductImportDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var categoryProducts = deserializedCategories.Select(x => mapper.Map<CategoryProductImportDTO, CategoryProduct>(x)).ToList();

            var categoriesIds = context.Categories.Select(x => x.Id).ToList();
            var productsIds = context.Products.Select(x => x.Id).ToList();

            var eligibleCategoryProducts = categoryProducts
                .Where(x => categoriesIds.Contains(x.CategoryId) && productsIds.Contains(x.ProductId));

            context.CategoryProducts.AddRange(eligibleCategoryProducts);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryImportDTO>), new XmlRootAttribute("Categories"));

            var deserializedCategories = new List<CategoryImportDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedCategories = (List<CategoryImportDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var categories = deserializedCategories.Select(x => mapper.Map<CategoryImportDTO, Category>(x)).ToList();

            context.Categories.AddRange(categories);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ProductImportDTO>), new XmlRootAttribute("Products"));

            var deserializedProducts = new List<ProductImportDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedProducts = (List<ProductImportDTO>)serializer.Deserialize(reader);
            }

            var a = deserializedProducts.Where(x => x.BuyerId == null).ToList();

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var products = deserializedProducts.Select(x => mapper.Map<ProductImportDTO, Product>(x)).ToList();

            context.Products.AddRange(products);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<UserImportDTO>), new XmlRootAttribute("Users"));

            var deserializedUsers = new List<UserImportDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedUsers = (List<UserImportDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var users = deserializedUsers.Select(x => mapper.Map<UserImportDTO, User>(x)).ToList();

            context.Users.AddRange(users);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }
    }
}