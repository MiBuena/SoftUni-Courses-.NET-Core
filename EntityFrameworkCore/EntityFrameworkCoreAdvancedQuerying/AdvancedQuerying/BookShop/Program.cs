using BookShop.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BookShop
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new BookShopContext())
            {
                context.Database.Migrate();
            }
        }
    }
}
