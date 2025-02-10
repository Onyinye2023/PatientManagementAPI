using Microsoft.EntityFrameworkCore;
using PatientManagement.Model.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PatientManagement.Dal.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Patient> Patients { get; set; }
        public DbSet<PatientRecord> PatientRecords { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Soft delete query filter
            modelBuilder.Entity<Patient>().HasQueryFilter(p => !p.IsDeleted);
            
            modelBuilder.Entity<PatientRecord>()
            .HasQueryFilter(pr => !pr.IsDeleted && !pr.Patient.IsDeleted);


            //Setting Patient Record primary key
            modelBuilder.Entity<PatientRecord>()
           .HasKey(pr => pr.RecordId);


            //To make sure emums are stored as string in the database
            modelBuilder.Entity<Patient>()
                .Property(p => p.BloodGroup)
                .HasConversion<string>();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Genotype)
                .HasConversion<string>();

            modelBuilder.Entity<Patient>()
                .Property(p => p.Gender)
                .HasConversion<string>();

            base.OnModelCreating(modelBuilder);
        }
    }
}
