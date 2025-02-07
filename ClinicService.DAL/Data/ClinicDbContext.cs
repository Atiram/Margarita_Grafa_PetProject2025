using ClinicService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Data
{
    public class ClinicDbContext : DbContext
    {
        public DbSet<DoctorEntity> Doctors { get; set; }
        public DbSet<PatientEntity> Patients { get; set; }
        public DbSet<AppoimentEntity> Appoiments { get; set; }

        public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
        {

            Database.EnsureCreated();
            if (Database.IsRelational())
            {
                Database.Migrate();
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Setup variables
            modelBuilder.Entity<DoctorEntity>(e =>
            {
                e.Property(b => b.DoctorId).HasMaxLength(50).IsRequired();
                e.Property(b => b.FirstName).HasMaxLength(100).IsRequired();
                e.Property(b => b.LastName).HasMaxLength(100).IsRequired();
                e.Property(b => b.MiddleName).HasMaxLength(100);
                e.Property(b => b.Email).HasMaxLength(100).IsRequired();
                e.Property(b => b.Specialization).HasMaxLength(100).IsRequired();
                e.Property(b => b.Office).HasMaxLength(100).IsRequired();
                e.Property(b => b.CareerStartYear).HasMaxLength(10).IsRequired();
                e.Property(b => b.Status).HasMaxLength(100).IsRequired();
            });

            modelBuilder.Entity<PatientEntity>(e =>
            {
                e.Property(b => b.PatientId).HasMaxLength(50).IsRequired();
                e.Property(b => b.FirstName).HasMaxLength(100).IsRequired();
                e.Property(b => b.LastName).HasMaxLength(100).IsRequired();
                e.Property(b => b.MiddleName).HasMaxLength(100);
                e.Property(b => b.PhoneNumber).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<AppoimentEntity>(e =>
            {
                e.Property(b => b.AppoimentId).HasMaxLength(50).IsRequired();
            });

            modelBuilder.Entity<DoctorEntity>()
                .HasKey(b => b.DoctorId);
            modelBuilder.Entity<PatientEntity>()
                .HasKey(b => b.PatientId);
            modelBuilder.Entity<AppoimentEntity>()
                .HasKey(b => b.AppoimentId);
            #endregion
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=ClinicApplication;Trusted_Connection=True;Encrypt=False;");
        }
    }
}
