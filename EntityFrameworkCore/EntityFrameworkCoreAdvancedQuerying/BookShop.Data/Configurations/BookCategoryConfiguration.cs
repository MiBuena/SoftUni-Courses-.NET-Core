using BookShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace BookShop.Data.Configurations
{
    public class BookCategoryConfiguration : IEntityTypeConfiguration<BookCategory>
    {
        public void Configure(EntityTypeBuilder<BookCategory> bookCategory)
        {
            bookCategory
                .HasKey(x => new { x.BookId, x.CategoryId });

            bookCategory
                .HasOne(x => x.Book)
                .WithMany(x => x.BookCategories)
                .HasForeignKey(x => x.BookId);

            bookCategory
                .HasOne(x => x.Category)
                .WithMany(x => x.CategoryBooks)
                .HasForeignKey(x => x.CategoryId);
        }
    }
}
