using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
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
                var inputJson = File.ReadAllText("./../../../Resources/ProductShop/categories.json");

                //var result = ImportCategories(context, inputJson);

                //Console.WriteLine(result);
            }
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
}