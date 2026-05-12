using Microsoft.EntityFrameworkCore;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Services;
using FoodServiceOccupancyForecast.Infrastructure.Data;
using FoodServiceOccupancyForecast.Infrastructure.Repositories;
using FoodServiceOccupancyForecast.VideoAnalysis.Processing;
using FoodServiceOccupancyForecast.Web.Hubs;
using FoodServiceOccupancyForecast.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// ===== SERVICES =====

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("FoodServiceOccupancyForecast.Infrastructure")
    )
);

// Repositories
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Services
builder.Services.AddScoped<IOccupancyService, OccupancyService>();
builder.Services.AddSingleton<VideoProcessingService>();

// Background service
builder.Services.AddHostedService<OccupancyBackgroundService>();

// SignalR
builder.Services.AddSignalR();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendPolicy", policy =>
    {
        policy.WithOrigins(
                builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
                ?? new[] { "http://localhost:3000", "http://localhost:5000", "http://localhost:5001" }
            )
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials();
    });
});

// Controllers + Razor Pages
builder.Services.AddControllers();
builder.Services.AddRazorPages();

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Food Service Occupancy API",
        Version = "v1",
        Description = "API для мониторинга загруженности ресторана Гулиновъ"
    });
});

// Auth (basic cookie auth for now)
builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Account/Login";
        options.AccessDeniedPath = "/Account/AccessDenied";
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
    options.AddPolicy("ManagerOrAdmin", policy => policy.RequireRole("Admin", "Manager"));
});

var app = builder.Build();

// ===== MIDDLEWARE =====

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Food Service Occupancy API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseHttpsRedirection();
app.UseCors("FrontendPolicy");
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Map endpoints
app.MapControllers();
app.MapRazorPages();
app.MapHub<OccupancyHub>("/occupancyHub");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.Run();
