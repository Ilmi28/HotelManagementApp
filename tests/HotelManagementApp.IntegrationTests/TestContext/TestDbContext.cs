using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HotelManagementApp.IntegrationTests.TestContext
{
    public class TestDbContext : HotelManagementAppDbContext
    {
        public TestDbContext(DbContextOptions options) : base(options) { }

        protected override void SeedData(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<IdentityRole>().HasData(
                new IdentityRole { Id = "1", Name = "User", NormalizedName = "USER" },
                new IdentityRole { Id = "2", Name = "Admin", NormalizedName = "ADMIN" },
                new IdentityRole { Id = "3", Name = "Manager", NormalizedName = "MANAGER" },
                new IdentityRole { Id = "4", Name = "Worker", NormalizedName = "WORKER" });
        }
    }
}
