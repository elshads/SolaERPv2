using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SolaERPv2.Server.Identity;

public class AppDbContext : IdentityDbContext<AppUser, AppRole, int>
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.HasDefaultSchema("Config");
        modelBuilder.Entity<AppUser>(entity => { entity.ToTable(name: "AppUser"); });
        modelBuilder.Entity<AppRole>(entity => { entity.ToTable(name: "AppRole"); });
    }
}
