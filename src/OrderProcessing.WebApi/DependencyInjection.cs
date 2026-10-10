using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using OrderProcessing.Application.Auth.Services;
using OrderProcessing.Application.Common.Behaviors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Products.Commands.CreateProduct;
using OrderProcessing.Infrastructure.Authentication;
using OrderProcessing.Infrastructure.Services;
using OrderProcessing.Persistence.Identity.Models;
using OrderProcessing.Persistence.Identity.Services;
using OrderProcessing.Persistence.Persistence;
using Stripe;

namespace OrderProcessing.WebApi;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        // الفهرس الرئيسي لتسجيل الخدمات
        services.AddDatabase(configuration);
        services.AddIdentityConfiguration();
        services.AddApplicationServices();
        services.AddWebApiServices();
        services.AddPaymentServices(configuration);
        services.AddJwtAuthentication(configuration);

        return services;
    }

    private static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IApplicationDbContext>(provider =>
            provider.GetRequiredService<AppDbContext>());

        return services;
    }

    private static IServiceCollection AddIdentityConfiguration(this IServiceCollection services)
    {
        services.AddIdentity<ApplicationUser, IdentityRole>(options =>
        {
            options.Password.RequireDigit = false;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
        })
        .AddEntityFrameworkStores<AppDbContext>()
        .AddDefaultTokenProviders();

        return services;
    }

    private static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Behaviors
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));

        // MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

        // FluentValidation
        services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);
        services.AddFluentValidationAutoValidation(); // للـ MVC Controllers

        return services;
    }

    private static IServiceCollection AddWebApiServices(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddEndpointsApiExplorer();

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                // 1. تعريف نوع الحماية
                var bearerScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "Enter your JWT token here."
                };

                // 2. إضافته كـ Component باستخدام الـ Interface الجديد
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes = new Dictionary<string, IOpenApiSecurityScheme>
                {
                    ["Bearer"] = bearerScheme
                };

                // 3. إنشاء متطلب الأمان (استخدام الكائن نفسه كمرجع)
                var requirement = new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference("Bearer")] = []
                };

                // 4. تمت إعادة تسمية SecurityRequirements إلى Security في النسخة الجديدة
                document.Security ??= [];
                document.Security.Add(requirement);

                return Task.CompletedTask;
            });
        });

        return services;
    }

    private static IServiceCollection AddPaymentServices(this IServiceCollection services, IConfiguration configuration)
    {
        StripeConfiguration.ApiKey = configuration["Stripe:SecretKey"];
        services.AddScoped<IPaymentService, StripePaymentService>();

        return services;
    }

    private static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddScoped<IAuthService, AuthService>();

        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));
        services.AddSingleton<IJwtProvider, JwtProvider>();

        return services;
    }
}