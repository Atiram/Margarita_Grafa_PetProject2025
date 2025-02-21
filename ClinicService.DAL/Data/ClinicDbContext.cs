using ClinicService.DAL.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicService.DAL.Data;
public class ClinicDbContext : DbContext
{
    public DbSet<DoctorEntity> Doctors { get; set; }
    public DbSet<PatientEntity> Patients { get; set; }
    public DbSet<AppointmentEntity> Appointments { get; set; }

    public ClinicDbContext(DbContextOptions<ClinicDbContext> options) : base(options)
    {
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
            e.Property(b => b.Id).HasMaxLength(100).IsRequired();
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
            e.Property(b => b.Id).HasMaxLength(50).IsRequired();
            e.Property(b => b.FirstName).HasMaxLength(100).IsRequired();
            e.Property(b => b.LastName).HasMaxLength(100).IsRequired();
            e.Property(b => b.MiddleName).HasMaxLength(100);
            e.Property(b => b.PhoneNumber).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<AppointmentEntity>(e =>
        {
            e.Property(b => b.Id).HasMaxLength(50).IsRequired();
        });

        modelBuilder.Entity<DoctorEntity>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<PatientEntity>()
            .HasKey(b => b.Id);
        modelBuilder.Entity<AppointmentEntity>()
            .HasKey(b => b.Id);
        #endregion
    }
}
