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
    }
}
