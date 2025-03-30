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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserLog>().Property(x => x.Operation).HasConversion<string>();

            SeedData(modelBuilder);
        }

        protected virtual void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Id = "4", Name = "Worker", NormalizedName = "WORKER"});

            modelBuilder.Ignore<IdentityUserClaim<Guid>>();
            modelBuilder.Ignore<IdentityUserLogin<Guid>>(); 
            modelBuilder.Ignore<IdentityUserToken<Guid>>();
            modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        }

    }
}
