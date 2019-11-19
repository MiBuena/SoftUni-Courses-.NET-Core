using Microsoft.EntityFrameworkCore;
using P03_SalesDatabase.Data;
using System;
using System.Linq;

namespace P03_SalesDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new SalesContext())
            {
                var a = context.Products.GroupBy(x => x.Name).ToList();
            }
        }
    }
}
