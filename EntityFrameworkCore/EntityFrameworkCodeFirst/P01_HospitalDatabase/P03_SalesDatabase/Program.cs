using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using System;

namespace P03_SalesDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SalesContext())
            {
                context.Database.Migrate();
            }
        }
    }
}
