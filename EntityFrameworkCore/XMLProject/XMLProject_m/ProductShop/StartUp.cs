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
                string xmlString = System.IO.File.ReadAllText("./../../../Resources/products.xml");

                //var result = ImportProducts(context, xmlString);

                //Console.WriteLine(result);
            }
        }

        public static string ImportProducts(ProductShopContext context, string inputXml)
        {
            var serializer = new XmlSerializer(typeof(List<ProductDTO>), new XmlRootAttribute("Products"));

            var deserializedProducts = new List<ProductDTO>();

            using (var reader = new StringReader(inputXml))
            {
                deserializedProducts = (List<ProductDTO>)serializer.Deserialize(reader);
            }

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
            var serializer = new XmlSerializer(typeof(List<UserDTO>), new XmlRootAttribute("users"));

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