using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P01_StudentSystem.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_StudentSystem.Data.Configurations
{
    public class StudentConfiguration : IEntityTypeConfiguration<Student>
    {
        public void Configure(EntityTypeBuilder<Student> student)
        {
            student
                .Property(t => t.Name)
                .IsUnicode(true);

            student
                .Property(e => e.PhoneNumber)
                .HasMaxLength(10)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
