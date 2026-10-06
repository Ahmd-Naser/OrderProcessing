using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OrderProcessing.Application.Common.Behaviors;
using OrderProcessing.Application.Common.Interfaces;
using OrderProcessing.Application.Products.Commands.CreateProduct;
using OrderProcessing.Infrastructure.Services;
using OrderProcessing.Persistence.Identity;
using OrderProcessing.Persistence.Persistence;
using Scalar.AspNetCore;
using Stripe;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();


// 1. تسجيل AppDbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));



// 2. تسجيل ASP.NET Core Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
})
.AddEntityFrameworkStores<AppDbContext>()
.AddDefaultTokenProviders();

builder.Services.AddScoped<IApplicationDbContext>(provider =>
    provider.GetRequiredService<AppDbContext>());

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(TransactionBehavior<,>));
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(IdempotencyBehavior<,>));


// تسجيل MediatR (بيروح يدور على الـ Handlers في مشروع الـ Application)
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(CreateProductCommand).Assembly));

// تسجيل FluentValidation (بيروح يدور على الـ Validators)
builder.Services.AddValidatorsFromAssembly(typeof(CreateProductCommand).Assembly);
// لو محتاج تفعيل الـ Validation التلقائي مع الـ MVC Controllers:
builder.Services.AddFluentValidationAutoValidation();


StripeConfiguration.ApiKey = builder.Configuration["Stripe:SecretKey"];

// تسجيل الخدمة
builder.Services.AddScoped<IPaymentService, StripePaymentService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}
app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

