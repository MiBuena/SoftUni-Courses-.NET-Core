using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data;
using System;

namespace P01_HospitalDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new HospitalContext())
            {
                context.Database.Migrate();
            }
        }
    }
}
