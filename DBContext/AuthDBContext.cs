using Employee.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Employee.DBContext;

public class AuthDbContext : IdentityDbContext<IdentityUser>
{
    
    public AuthDbContext(DbContextOptions<AuthDbContext> options) : base(options) { }
    
    public AuthDbContext() {}

    private DbSet<User> User { get; set; } = null;
    private DbSet<RefreshToken> RefreshTokens { get; set; } = null;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<User>().ToTable("User"); // creating user table
        modelBuilder.Entity<RefreshToken>().ToTable("RefreshToken");
        
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "2", Name = "User", NormalizedName = "USER" }
        );
    }
}