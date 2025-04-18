using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.Infrastructure.Database;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<UserLog> AccountLogs { get; set; }
    public DbSet<BlacklistedGuest> BlackListedGuests { get; set; }
    public DbSet<AccountOperation> AccountHistory { get; set; }
    public DbSet<VIPGuest> VIPGuests { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<ProfilePicture> ProfilePictures { get; set; }
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
            new IdentityRole { Id = "1", Name = "Guest", NormalizedName = "GUEST" },
            new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" },
            new IdentityRole { Id = "4", Name = "Staff", NormalizedName = "STAFF" });

        modelBuilder.Entity<AccountOperation>().HasData(
            new AccountOperation { Id = 1, Name = "REGISTER" },
            new AccountOperation { Id = 2, Name = "LOGIN" },
            new AccountOperation { Id = 3, Name = "CREATE" },
            new AccountOperation { Id = 4, Name = "UPDATE" },
            new AccountOperation { Id = 5, Name = "DELETE" },
            new AccountOperation { Id = 6, Name = "PASSWORD CHANGE" });

    }

}
