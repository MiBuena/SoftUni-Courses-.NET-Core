using CarDealer.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using System;

namespace CarDealer.Data
{
    public class CarDealerContext : DbContext
    {
        //public CarDealerContext(DbContextOptions options)
        //    : base(options)
        //{
        //}

        public CarDealerContext()
        {
        }

        public DbSet<Car> Cars { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<PartCar> PartCars { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=DESKTOP-EFGPD5V\\SQLEXPRESS;Database=CarDealer;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PartCar>(e =>
            {
                e.HasKey(k => new { k.CarId, k.PartId });
            });

            //modelBuilder
            //    .Entity<PartCar>()
            //    .HasOne(k => k.Part)
            //    .WithMany(x => x.PartCars)
            //    .HasForeignKey(x => x.PartId);

            //modelBuilder
            //    .Entity<PartCar>()
            //    .HasOne(k => k.Car)
            //    .WithMany(x => x.PartCars)
            //    .HasForeignKey(x => x.CarId);


            //modelBuilder
            //    .Entity<Sale>()
            //    .HasOne(k => k.Car)
            //    .WithMany(x => x.Sales)
            //    .HasForeignKey(x => x.CarId);

            //modelBuilder
            //    .Entity<Sale>()
            //    .HasOne(k => k.Customer)
            //    .WithMany(x => x.Sales)
            //    .HasForeignKey(x => x.CustomerId);

            //modelBuilder
            //    .Entity<Part>()
            //    .HasOne(k => k.Supplier)
            //    .WithMany(x => x.Parts)
            //    .HasForeignKey(x => x.SupplierId);
        }
    }
}
