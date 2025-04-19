using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;

namespace HotelManagementApp.API;

public static class DependencyInjection
{
    public static IHostApplicationBuilder AddApiServices(this IHostApplicationBuilder builder)
    {
        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddProblemDetails();

        builder.Services.AddSwaggerGen(x =>
        {
            var security = new OpenApiSecurityScheme
            {
                Name = HeaderNames.Authorization,
                Type = SecuritySchemeType.ApiKey,
                In = ParameterLocation.Header,
                Description = "JWT Authorization header",
                Reference = new OpenApiReference
                {
                    Id = JwtBearerDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                }
            };

            x.AddSecurityDefinition(security.Reference.Id, security);
            x.AddSecurityRequirement(new OpenApiSecurityRequirement
            {{security, Array.Empty<string>()}});
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            x.IncludeXmlComments(xmlPath);
        });

        return builder;
    }
}
