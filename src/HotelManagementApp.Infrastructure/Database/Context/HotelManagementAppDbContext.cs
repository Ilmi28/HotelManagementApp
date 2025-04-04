using HotelManagementApp.Core.Models;
using HotelManagementApp.Infrastructure.Database.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.Infrastructure.Database.Context
{
    public class HotelManagementAppDbContext : IdentityDbContext<User>
    {
        public HotelManagementAppDbContext(DbContextOptions options) : base(options) { }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<UserLog> UserLogs { get; set; }
        public DbSet<BlacklistedUsers> BlackListedUsers { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<VIPUsers> VIPUsers { get; set; }
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
                new IdentityRole { Id = "4", Name = "Worker", NormalizedName = "WORKER"});

            modelBuilder.Entity<Operation>().HasData(
                new Operation { Id = 1, Name = "REGISTER" },
                new Operation { Id = 2, Name = "LOGIN" },
                new Operation { Id = 3, Name = "CREATE" },
                new Operation { Id = 4, Name = "UPDATE" },
                new Operation { Id = 5, Name = "DELETE" },
                new Operation { Id = 6, Name = "PASSWORD CHANGE" });

        }

    }
}
