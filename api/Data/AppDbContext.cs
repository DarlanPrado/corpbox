using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var userEntity = modelBuilder.Entity<User>();

        userEntity.ToTable("Users");
        userEntity.HasKey(x => x.Id);
        userEntity.HasIndex(x => x.Email).IsUnique();
        userEntity.Property(x => x.Name).HasMaxLength(120).IsRequired();
        userEntity.Property(x => x.Email).HasMaxLength(255).IsRequired();
        userEntity.Property(x => x.PasswordHash).HasMaxLength(512).IsRequired();
        userEntity.Property(x => x.SessionTokenHash).HasMaxLength(128);
        userEntity.Property(x => x.Created).IsRequired();
        userEntity.Property(x => x.Updated).IsRequired();

        userEntity.HasQueryFilter(x => x.Deleted == null);
    }
}
