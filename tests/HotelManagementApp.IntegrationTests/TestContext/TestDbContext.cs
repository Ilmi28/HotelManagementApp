using HotelManagementApp.Infrastructure.Database.Context;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.IntegrationTests.TestContext;

public class TestDbContext(DbContextOptions options) : HotelManagementAppDbContext(options)
{
}
