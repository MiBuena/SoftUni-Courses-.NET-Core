using BookShop.Data;
using BookShop.Models.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace BookShop
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            using (var context = new BookShopContext())
            {
                //context.Database.Migrate();

                var result = GetBooksByAgeRestriction(context, "teEN");

                Console.WriteLine(result);
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            AgeRestriction myEnum = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);

            var titles = context.Books
                .Where(x => x.AgeRestriction == myEnum)
                .OrderBy(x=>x.Title)
                .Select(x => x.Title)
                .ToList();

            var result = string.Join('\n', titles);

            return result;
        }
    }
}
