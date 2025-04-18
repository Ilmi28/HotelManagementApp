using HotelManagementApp.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementApp.IntegrationTests.TestContext;

public class TestDbContext(DbContextOptions options) : AppDbContext(options)
{
}
