using Microsoft.EntityFrameworkCore;
using TrainingWebAPI.Models;

namespace TrainingWebAPI.Data;

public class AppDbContext : DbContext {
    public DbSet<User> Users { get; set; } = null!;

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
         base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToTable("users", table =>
            table.HasCheckConstraint("users_age_check", "age > 0"));
        modelBuilder.Entity<User>().HasKey(user => user.Id)
            .HasName("users_pkey");

        modelBuilder.Entity<User>()
            .Property(user => user.Id)
            .HasColumnName("id");

        modelBuilder.Entity<User>()
            .Property(user => user.Name)
            .HasColumnName("name")
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<User>()
            .Property(user => user.Age)
            .HasColumnName("age");
        
        modelBuilder.Entity<User>()
            .Property(user => user.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(false);
    }
}
