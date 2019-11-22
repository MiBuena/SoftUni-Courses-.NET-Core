using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.Configurations
{
    public class AuthorConfiguration : IEntityTypeConfiguration<Author>
    {
        public void Configure(EntityTypeBuilder<Author> author)
        {
            author
                .Property(x => x.FirstName)
                .IsUnicode(true);

            author
                .Property(x => x.LastName)
                .IsUnicode(true);
        }
    }
}
