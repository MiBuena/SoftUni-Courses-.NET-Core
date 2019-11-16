using Microsoft.EntityFrameworkCore;
using P01_HospitalDatabase.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace P01_HospitalDatabase.Data
{
    public class HospitalContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }

        public DbSet<Visitation> Visitations { get; set; }

        public DbSet<Diagnose> Diagnoses { get; set; }

        public DbSet<Medicament> Medicaments { get; set; }

        public DbSet<PatientMedicament> PatientsMedicaments { get; set; }


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=.;Database=HospitalContext;Integrated Security=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Patient>(entity =>
            {
                entity.Property(e => e.FirstName).IsUnicode(true);
                entity.Property(e => e.LastName).IsUnicode(true);
                entity.Property(e => e.Address).IsUnicode(true);
                entity.Property(e => e.Email).IsUnicode(false);
            });

            modelBuilder.Entity<Visitation>(entity =>
            {
                entity.Property(e => e.Comments).IsUnicode(true);
            });

            modelBuilder
                .Entity<Visitation>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.Visitations)
                .HasForeignKey(x => x.PatientId);

            modelBuilder.Entity<Diagnose>(entity =>
            {
                entity.Property(e => e.Name).IsUnicode(true);
                entity.Property(e => e.Comments).IsUnicode(true);
            });

            modelBuilder
                .Entity<Diagnose>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.Diagnoses)
                .HasForeignKey(x => x.PatientId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasKey(x => new { x.PatientId, x.MedicamentId });

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(x => x.Medicament)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.MedicamentId);

            modelBuilder
                .Entity<PatientMedicament>()
                .HasOne(x => x.Patient)
                .WithMany(x => x.Prescriptions)
                .HasForeignKey(x => x.PatientId);
        }
    }
}
