using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using P03_SalesDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P03_SalesDatabase.Data.Configurations
{
    public class SaleConfiguration : IEntityTypeConfiguration<Sale>
    {
        public void Configure(EntityTypeBuilder<Sale> sale)
        {
            sale
              .HasOne(x => x.Product)
              .WithMany(x => x.Sales)
              .HasForeignKey(x => x.ProductId)
              .OnDelete(DeleteBehavior.Restrict);

            sale
                .HasOne(x => x.Customer)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);

            sale
                .HasOne(x => x.Store)
                .WithMany(x => x.Sales)
                .HasForeignKey(x => x.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            sale
                .Property(b => b.Date)
                .HasDefaultValueSql("GetDate()");
        }
    }
}

