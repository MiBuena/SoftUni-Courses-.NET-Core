using System;
using System.Linq;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using Z.EntityFramework.Plus;

namespace P01_StudentSystem
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var context = new StudentSystemContext())
            {
                var students = context
                    .Students
                    .Where(x => x.PhoneNumber == null)
                    .Update(x => new Student()
                    {
                        Name = x.Name + "aaaa"
                    });



            }
        }
    }
}
