using Api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Api.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
    : IdentityDbContext<AppUser>(options)
{
    public DbSet<Stock> Stock { get; set; }

    public DbSet<Comment> Comment { get; set; }

    public DbSet<Portfolio> Portfolios { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Portfolio>((e) => e.HasKey((p) => new { p.AppUserId, p.StockId }));

        builder
            .Entity<Portfolio>()
            .HasOne((u) => u.AppUser)
            .WithMany((u) => u.Portfolios)
            .HasForeignKey((p) => p.AppUserId);

        builder
            .Entity<Portfolio>()
            .HasOne((u) => u.Stock)
            .WithMany((u) => u.Portfolios)
            .HasForeignKey((p) => p.StockId);

        List<IdentityRole> roles =
        [
            new()
            {
                Name = "Admin",
                NormalizedName = "ADMIN",
                Id = "admin-role-ID",
            },
            new()
            {
                Name = "User",
                NormalizedName = "USER",
                Id = "user-role-ID",
            },
        ];

        builder.Entity<IdentityRole>().HasData(roles);
    }
}
