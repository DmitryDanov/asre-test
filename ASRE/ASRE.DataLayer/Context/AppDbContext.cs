using ASRE.DataLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace ASRE.DataLayer.Context;

// The DbContext is going to be used explicitly through the code for this test project but apparently it would be better to create a repository.
public sealed class AppDbContext : DbContext
{
    public DbSet<Patient> Patients { get; set; }

    public DbSet<PatientGivenName> PatientGivenNames { get; set; }

    public DbSet<Gender> Genders { get; set; }

    public DbSet<Activation> Activations { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">An instance of a <see cref="DbContextOptions"/> class.</param>
    public AppDbContext(DbContextOptions options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Patient>(entity =>
        {
            entity.HasOne(p => p.Gender).WithMany(p => p.Patients).HasForeignKey(p => p.GenderId);
            entity.HasOne(p => p.Activation).WithMany(p => p.Patients).HasForeignKey(p => p.ActivationId);
            entity.HasMany(p => p.PatientGivenNames).WithOne(p => p.Patient).HasForeignKey(p => p.PatientId);

            // Creating BirthDate of datetime instead of datetime2 as we don't need additional precision and support of min values of 01/01/0001.
            entity.Property(p => p.BirthDate).HasColumnType("datetime");
        });

        modelBuilder.Entity<PatientGivenName>();

        modelBuilder.Entity<Gender>(entity =>
        {
            // Gender dictionary table data.
            entity.HasData(
                new Gender { Id = new Guid("38f4e1cc-440c-4e10-94c9-af1db3364f07"), Name = "unknown" },
                new Gender { Id = new Guid("7c610b7c-9a23-4f54-bf1f-e9d5d02b6b5d"), Name = "male" },
                new Gender { Id = new Guid("ecb9f689-9427-4e27-8c23-2ac2c3e3d0a5"), Name = "female" },
                new Gender { Id = new Guid("e97c5b5d-484c-43e9-8850-d65c57579875"), Name = "other" }
            );
        });

        modelBuilder.Entity<Activation>(entity =>
        {
            // Activation dictionary table data.
            entity.HasData(
                new Gender { Id = new Guid("350fda9b-d8e0-4df4-ad3f-9a374c9e0801"), Name = "true" },
                new Gender { Id = new Guid("920b297b-8827-45ca-9d9b-f3eea9f2fcb4"), Name = "false" }
            );
        });

        base.OnModelCreating(modelBuilder);
    }
}