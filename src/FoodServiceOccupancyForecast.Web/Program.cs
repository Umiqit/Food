using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Services;
using FoodServiceOccupancyForecast.Infrastructure.Data;
using FoodServiceOccupancyForecast.Infrastructure.Repositories;
using FoodServiceOccupancyForecast.Web.Hubs;
using FoodServiceOccupancyForecast.Web.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSignalR();

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Repositories
builder.Services.AddScoped<ITableRepository, TableRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

// Services
builder.Services.AddScoped<TableService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddHostedService<TableStatusService>();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors("AllowAll");
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<TableHub>("/tableHub");

// Seed database
using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    context.Database.Migrate();
}

app.Run();
