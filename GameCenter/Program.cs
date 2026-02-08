using CartService.Clients;
using CartService.DbModels;
using CartService.IClients;
using CartService.IRepositories;
using CartService.IServices;
using CartService.Options;
using CartService.Services;
using GameCenter.IRepositories;
using GameCenter.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<CartServiceDbContext>(options =>
    options.UseNpgsql(connectionString, npgsqlOptions =>
    {
        npgsqlOptions.MigrationsHistoryTable("__EFMigrationsHistory", "cart");
    }));

builder.Services.AddScoped<ICartRepository, CartRepository>();
builder.Services.AddScoped<IUserCartRepository, UserCartRepository>();

builder.Services.Configure<ProductServiceOptions>(
    builder.Configuration.GetSection(ProductServiceOptions.SectionName));

builder.Services.AddHttpClient<IProductServiceClient, ProductServiceClient>(client =>
{
    var options = builder.Configuration.GetSection(ProductServiceOptions.SectionName)
        .Get<ProductServiceOptions>() ?? new ProductServiceOptions();
    client.BaseAddress = new Uri(options.BaseUrl);
    client.Timeout = TimeSpan.FromSeconds(options.TimeoutSeconds);
});

// Регистрация подсервисов
builder.Services.AddScoped<CartService.Services.CartItemService>();
builder.Services.AddScoped<CartService.Services.CartTotalsService>();
builder.Services.AddScoped<CartService.Services.CartMappingService>();

// Регистрация основного сервиса
builder.Services.AddScoped<ICartService, CartService.Services.CartService>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "CartService API v1");
        c.RoutePrefix = string.Empty;
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.MapGet("/", () => "Hello World!");

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<CartServiceDbContext>();
    try
    {
        dbContext.Database.ExecuteSqlRaw("CREATE SCHEMA IF NOT EXISTS cart;");
        dbContext.Database.Migrate();
    }
    catch (Exception ex)
    {
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating or initializing the database.");
    }
}

app.Run();
