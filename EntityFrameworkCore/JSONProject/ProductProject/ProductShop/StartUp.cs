using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            using (var context = new ProductShopContext())
            {
                var result = GetUsersWithProducts(context);

                Console.WriteLine(result);
            }
        }

        public static string GetUsersWithProducts(ProductShopContext context)
        {

             var users = context.Users
                .Where(u => u.ProductsSold.Any(ps => ps.Buyer != null))
                .OrderByDescending(u => u.ProductsSold.Count(ps => ps.Buyer != null))
                .Select(u =>
                    new
                    {
                        FirstName = u.FirstName,
                        LastName = u.LastName,
                        Age = u.Age,
                        SoldProducts = new
                        {
                            Count = u.ProductsSold.Count(ps => ps.Buyer != null),
                            Products = u.ProductsSold
                                .Where(ps => ps.Buyer != null)
                                .Select(ps =>
                                    new
                                    {
                                        Name = ps.Name,
                                        Price = ps.Price
                                    })
                                .ToArray()
                        }
                    })
                .ToArray();

            var userProductDTO = new 
            {
                UsersCount = users.Count(),
                Users = users
            };

          
            var serializerSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new CamelCaseNamingStrategy(),
                },
                Formatting = Formatting.Indented
            };

            var jsonProduct = JsonConvert.SerializeObject(userProductDTO, serializerSettings);

            return jsonProduct;
        }

        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var categories = context.Categories
                .OrderByDescending(x => x.CategoryProducts.Count)
                .Select(x => new
                {
                    Category = x.Name,
                    ProductsCount = x.CategoryProducts.Count,
                    AveragePrice = Math.Round(x.CategoryProducts.Average(y => y.Product.Price), 2).ToString(),
                    TotalRevenue = Math.Round(x.CategoryProducts.Sum(y => y.Product.Price), 2).ToString(),
                })
                .ToList();


            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy(),
            };

            var newSettings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
                Formatting = Formatting.Indented
            };

            var jsonProduct = JsonConvert.SerializeObject(categories, newSettings);

            return jsonProduct;
        }

        public static string GetSoldProducts(ProductShopContext context)
        {
            var users = context.Users
                .Where(x => x.ProductsSold.Any(y => y.BuyerId != null))
                .OrderBy(x => x.LastName)
                .ThenBy(x => x.FirstName)
                .Select(x => new
                {
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    SoldProducts = x.ProductsSold.Select(y => new
                    {
                        Name = y.Name,
                        Price = y.Price,
                        BuyerFirstName = y.Buyer.FirstName,
                        BuyerLastName = y.Buyer.LastName
                    })
                })
                .ToList();


            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newSettings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
            };


            var jsonProduct = JsonConvert.SerializeObject(users, newSettings);


            return jsonProduct;
        }

        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context.Products
                .Where(x => x.Price >= 500 && x.Price <= 1000)
                .OrderBy(x => x.Price)
                .Select(x => new
                {
                    Name = x.Name,
                    Price = x.Price,
                    Seller = x.Seller.FirstName + " " + x.Seller.LastName
                })
                .ToList();


            var resolver = new DefaultContractResolver()
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            };

            var newSettings = new JsonSerializerSettings()
            {
                ContractResolver = resolver,
            };


            var jsonProduct = JsonConvert.SerializeObject(products, newSettings);


            return jsonProduct;
        }

        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var categoriesProducts = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.CategoryProducts.AddRange(categoriesProducts);

            var insertedRecords = context.SaveChanges();

            var result = $"Successfully imported {insertedRecords}";

            return result;
        }

        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var categories = JsonConvert.DeserializeObject<List<Category>>(inputJson)
                .Where(x => x.Name != null)
                .ToList();

            context.Categories.AddRange(categories);

            var insertedRecords = context.SaveChanges();

            var result = $"Successfully imported {insertedRecords}";

            return result;
        }

        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var products = JsonConvert.DeserializeObject<List<Product>>(inputJson);

            context.Products.AddRange(products);

            var insertedRecords = context.SaveChanges();

            var result = $"Successfully imported {insertedRecords}";

            return result;
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var users = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.AddRange(users);

            var insertedRecords = context.SaveChanges();

            var result = $"Successfully imported {insertedRecords}";

            return result;
        }
    }

    public class UserProductDTO
    {
        public UserProductDTO()
        {
            Users = new List<UserDTO>();
        }

        public int UserCount { get; set; }

        public List<UserDTO> Users { get; set; }
    }

    public class UserDTO
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public int? Age { get; set; }

        public SalesStatisticDTO SoldProducts { get; set; }


    }
       
    public class SalesStatisticDTO
    {
        public SalesStatisticDTO()
        {
            Products = new List<ProductDTO>();
        }

        public int Count { get; set; }

        public List<ProductDTO> Products { get; set; }
    }

    public class ProductDTO
    {
        public string Name { get; set; }

        public decimal Price { get; set; }
    }
}