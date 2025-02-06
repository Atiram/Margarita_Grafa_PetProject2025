using System.Reflection.Metadata;
using ClinicService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Data
{
    internal class ClinicDbContext : DbContext
    {
        public DbSet<DoctorEntity> Doctors { get; set; }
        public DbSet<PatientEntity> Patients { get; set; }
        public DbSet<AppoimentEntity> Appoiments { get; set; }

        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DoctorEntity>()
                .Property(b => b.DoctorId)
                .IsRequired();
            modelBuilder.Entity<DoctorEntity>()
                .HasKey(b => b.DoctorId);
             
                

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ClinicApplication;Trusted_Connection=True;Encrypt=False;");
        }


    }
}
