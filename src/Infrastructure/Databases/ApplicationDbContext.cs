using Domain.Entities;
using EntityFramework.Exceptions.PostgreSQL;
using Infrastructure.Databases.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Databases;

public class ApplicationDbContext(DbContextOptions options) : DbContext(options)
{
    private static readonly TimestampsInterceptor TimestampsInterceptor = new();

    public DbSet<Profile> Profiles { get; set; } = null!;
    public DbSet<PlayerStats> PlayerStats { get; set; } = null!;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(TimestampsInterceptor);

        optionsBuilder.UseExceptionProcessor();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Profile>()
            .HasIndex(p => p.Nickname)
            .IsUnique();

        modelBuilder.Entity<Profile>()
            .HasIndex(p => p.UserId)
            .IsUnique();
    }
}