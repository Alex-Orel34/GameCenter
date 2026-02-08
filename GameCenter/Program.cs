using CartService.DbModels;
using CartService.IRepositories;
using CartService.IServices;
using CartService.Services;
using GameCenter.IRepositories;
using GameCenter.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
builder.Services.AddControllers();

// Настройка Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "CartService API",
        Version = "v1",
        Description = "API для управления корзиной"
    });
});

// Настройка PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<CartServiceDbContext>(options =>
    options.UseNpgsql(connectionString));

// Регистрация репозитория
builder.Services.AddScoped<ICartRepository, CartRepository>();

// Регистрация сервиса
builder.Services.AddScoped<IUserCartRepository, UserCartRepository>();

// Регистрация сервиса корзины
builder.Services.AddScoped<ICartService, CartService>();

// Настройка пайплайна
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Training Platform API v1");
        c.RoutePrefix = string.Empty; // Swagger UI будет доступен по корневому пути
    });
}

app.MapGet("/", () => "Hello World!");

app.Run();
