using Microsoft.EntityFrameworkCore;

namespace AuthService.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) {}

    public DbSet<User> Users { get; set; }
    public DbSet<UserToken> UserTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Map users table
        modelBuilder.Entity<User>(entity =>
        {
            entity.ToTable("users");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Email).HasColumnName("email");
            entity.Property(e => e.PasswordHash).HasColumnName("password_hash");
            entity.Property(e => e.CreatedAt).HasColumnName("created_at");
        });

        // Map user_tokens table
        modelBuilder.Entity<UserToken>(entity =>
        {
            entity.ToTable("user_tokens");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.UserId).HasColumnName("user_id");
            entity.Property(e => e.DeviceId).HasColumnName("device_id");
            entity.Property(e => e.RefreshToken).HasColumnName("refresh_token");
            entity.Property(e => e.ExpiresAt).HasColumnName("expires_at");
        });
    }
}

// User Model
public class User
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }
    public DateTime CreatedAt { get; set; }
}

// UserToken Model
public class UserToken
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string DeviceId { get; set; }
    public string RefreshToken { get; set; }
    public DateTime ExpiresAt { get; set; }
}

