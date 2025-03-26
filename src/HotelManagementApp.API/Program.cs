
using HotelManagementApp.API.ExtensionMethods;

namespace HotelManagementApp.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            ConfigureServices(builder);

            var app = builder.Build();

            ConfigureMiddleware(app);

            app.MapControllers();
            app.Run();
        }

        public static void ConfigureServices(WebApplicationBuilder builder)
        {
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddRepositories();
            builder.Services.AddServices();
            builder.Services.AddJwtBearer(builder.Configuration);
            builder.Services.AddIdentityDbContext(builder.Configuration);
        }

        public static void ConfigureMiddleware(WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseHttpsRedirection();
            app.UseAuthorization();
        }
    }

}
