using HotelManagementApp.Core.Enums;
using HotelManagementApp.Core.Models;
using HotelManagementApp.Core.Models.AccountModels;
using HotelManagementApp.Core.Models.DiscountModels;
using HotelManagementApp.Core.Models.GuestModels;
using HotelManagementApp.Core.Models.HotelModels;
using HotelManagementApp.Core.Models.LoyaltyPointsModels;
using HotelManagementApp.Core.Models.OrderModels;
using HotelManagementApp.Core.Models.PaymentModels;
using HotelManagementApp.Core.Models.RoleModels;
using HotelManagementApp.Core.Models.TokenModels;
using HotelManagementApp.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace HotelManagementApp.Infrastructure.Database;

public class AppDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AccountLog> AccountHistory { get; set; }
    public DbSet<BlacklistedGuest> BlackListedGuests { get; set; }
    public DbSet<AccountOperation> AccountOperations { get; set; }
    public DbSet<VIPGuest> VIPGuests { get; set; }
    public DbSet<ResetPasswordToken> ResetPasswordTokens { get; set; }
    public DbSet<ConfirmEmailToken> ConfirmEmailTokens { get; set; }
    public DbSet<Hotel> Hotels { get; set; }
    public DbSet<HotelRoom> HotelRooms { get; set; }
    public DbSet<HotelRoomType> HotelRoomTypes { get; set; }
    public DbSet<ProfilePicture> ProfilePictures { get; set; }
    public DbSet<HotelImage> HotelImages { get; set; }
    public DbSet<HotelService> HotelServices { get; set; }
    public DbSet<HotelParking> HotelParkings { get; set; }
    public DbSet<HotelRoomImage> HotelRoomImages { get; set; }
    public DbSet<HotelReview> HotelReviews { get; set; }
    public DbSet<City> Cities { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<LoyaltyPoints> LoyaltyPoints { get; set; }
    public DbSet<LoyaltyPointsLog> LoyaltyPointsHistory { get; set; }
    public DbSet<LoyaltyReward> LoyaltyRewards { get; set; }
    public DbSet<PaymentMethod> PaymentMethods { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderDetails> OrderDetails { get; set; }
    public DbSet<PendingOrder> PendingOrders { get; set; }
    public DbSet<ConfirmedOrder> ConfirmedOrders { get; set; }
    public DbSet<CancelledOrder> CancelledOrders { get; set; }
    public DbSet<CompletedOrder> CompletedOrders { get; set; }
    public DbSet<OrderStatus> OrderStatuses { get; set; }
    public DbSet<RoomDiscount> RoomDiscounts { get; set; }
    public DbSet<DiscountCode> DiscountCodes { get; set; }
    public DbSet<HotelDiscount> HotelDiscounts { get; set; }
    public DbSet<ServiceDiscount> ServiceDiscounts { get; set; }
    public DbSet<ParkingDiscount> ParkingDiscounts { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();

        modelBuilder.Entity<Order>()
        .HasOne(o => o.OrderDetails)
        .WithOne(d => d.Order)
        .HasForeignKey<OrderDetails>(x => x.OrderId);

        SeedData(modelBuilder);
    }

    protected virtual void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<IdentityRole>().HasData(
            new IdentityRole { Id = "1", Name = "Guest", NormalizedName = "GUEST" },
            new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" },
            new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" },
            new IdentityRole { Id = "4", Name = "Staff", NormalizedName = "STAFF" });

        modelBuilder.Entity<AccountOperation>().HasData(EnumModel<AccountOperationEnum>.ParseEnumsToModel());
        modelBuilder.Entity<HotelRoomType>().HasData(EnumModel<RoomTypeEnum>.ParseEnumsToModel());
        modelBuilder.Entity<PaymentMethod>().HasData(EnumModel<PaymentMethodEnum>.ParseEnumsToModel());
        modelBuilder.Entity<OrderStatus>().HasData(EnumModel<OrderStatusEnum>.ParseEnumsToModel());
        /*modelBuilder.Entity<AccountOperation>().HasData(
            new AccountOperation { Id = 1, Name = "REGISTER" },
            new AccountOperation { Id = 2, Name = "LOGIN" },
            new AccountOperation { Id = 3, Name = "CREATE" },
            new AccountOperation { Id = 4, Name = "UPDATE" },
            new AccountOperation { Id = 5, Name = "DELETE" },
            new AccountOperation { Id = 6, Name = "PASSWORD CHANGE" });*/

    }

}
