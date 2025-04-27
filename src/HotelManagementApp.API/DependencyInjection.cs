using HotelManagementApp.Application.Policies.AccountOwnerPolicy;
using HotelManagementApp.Application.Policies.RoleHierarchyPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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

        builder.Services.AddScoped<IAuthorizationHandler, AccountOwnerHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, RoleHierarchyHandler>();
        builder.Services.AddAuthentication(config =>
        {
            config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultForbidScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        });
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AccountOwner", policy =>
                policy.Requirements.Add(new AccountOwnerRequirement()));
            options.AddPolicy("RoleHierarchy", policy =>
                policy.Requirements.Add(new RoleHierarchyRequirement()));
        });

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
