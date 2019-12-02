using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ProductShop.Data;
using ProductShop.DTOs;
using ProductShop.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                string xmlString = System.IO.File.ReadAllText("./../../../Datasets/products.xml");

                //context.Database.Migrate();
                var result = ImportProducts(context, xmlString);

                Console.WriteLine(result);
            }
        }

        //public static string ImportCategoryProducts(ProductShopContext context, string inputXml)
        //{

        //}

        public static string ImportCategories(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<CategoryDTO>), new XmlRootAttribute("Categories"));
            
            var deserializedCategories = new List<CategoryDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedCategories = (List<CategoryDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var categories = deserializedCategories.Select(x => mapper.Map<CategoryDTO, Category>(x)).ToList();

            context.Categories.AddRange(categories);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ProductDTO>), new XmlRootAttribute("Products"));

            var deserializedProducts = new List<ProductDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedProducts = (List<ProductDTO>)serializer.Deserialize(reader);
            }

            var a = deserializedProducts.Where(x => x.BuyerId == null).ToList();

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var products = deserializedProducts.Select(x => mapper.Map<ProductDTO, Product>(x)).ToList();

            context.Products.AddRange(products);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;
        }

        public static string ImportUsers(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<UserDTO>), new XmlRootAttribute("Users"));

            var deserializedUsers = new List<UserDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedUsers = (List<UserDTO>)serializer.Deserialize(reader);
            }

            var config = new MapperConfiguration(cfg => {
                cfg.AddProfile<ProductShopProfile>();
            });

            IMapper mapper = new Mapper(config);
            var users = deserializedUsers.Select(x => mapper.Map<UserDTO, User>(x)).ToList();

            context.Users.AddRange(users);
            var count = context.SaveChanges();

            var result = $"Successfully imported {count}";

            return result;        
        }
    }
}