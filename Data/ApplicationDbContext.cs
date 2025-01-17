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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

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
