using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Database.Context;

public class HotelManagementAppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserLog> UserLogs { get; set; }
    public DbSet<BlacklistedUser> BlackListedUsers { get; set; }
    public DbSet<Operation> Operations { get; set; }
    public DbSet<VIPUser> VIPUsers { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();

        SeedData(modelBuilder);
    }

    protected virtual void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Client", NormalizedName = "CLIENT" },
            new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" },
            new IdentityRole { Id = "4", Name = "Worker", NormalizedName = "WORKER" });

        modelBuilder.Entity<Operation>().HasData(
            new Operation { Id = 1, Name = "REGISTER" },
            new Operation { Id = 2, Name = "LOGIN" },
            new Operation { Id = 3, Name = "CREATE" },
            new Operation { Id = 4, Name = "UPDATE" },
            new Operation { Id = 5, Name = "DELETE" },
            new Operation { Id = 6, Name = "PASSWORD CHANGE" });

    }

}
