using HotelManagementApp.API.Policies.AccountOwnerPolicy;
using HotelManagementApp.API.Policies.RoleHierarchyPolicy;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Net.Http.Headers;
using Microsoft.OpenApi.Models;
using System.Reflection;
using HotelManagementApp.API.Policies.ConfirmedEmailPolicy;
using HotelManagementApp.API.Policies.OrderAccessPolicy;
using HotelManagementApp.API.Policies.PaymentAccessPolicy;
using HotelManagementApp.API.Policies.ReservationAccessPolicy;
using HotelManagementApp.API.Policies.ReviewAccessPolicy;
using HotelManagementApp.Core.Models.PaymentModels;

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
        builder.Services.AddScoped<IAuthorizationHandler, OrderAccessHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, ReservationAccessHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, PaymentAccessHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, ConfirmedEmailHandler>();
        builder.Services.AddScoped<IAuthorizationHandler, ReviewAccessHandler>();
        builder.Services.AddAuthorization(options =>
        {
            options.AddPolicy("AccountOwner", policy =>
                policy.Requirements.Add(new AccountOwnerRequirement()));
            options.AddPolicy("RoleHierarchy", policy =>
                policy.Requirements.Add(new RoleHierarchyRequirement()));
            options.AddPolicy("OrderAccess", policy =>
                policy.Requirements.Add(new OrderAccessRequirement()));
            options.AddPolicy("ReservationAccess", policy =>
                policy.Requirements.Add(new ReservationAccessRequirement()));
            options.AddPolicy("PaymentAccess", policy =>
                policy.Requirements.Add(new PaymentAccessRequirement()));
            options.AddPolicy("EmailConfirmed", policy => 
                policy.Requirements.Add(new ConfirmedEmailRequirement()));
            options.AddPolicy("ReviewAccess", policy => 
                policy.Requirements.Add(new ReviewAccessRequirement()));
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
