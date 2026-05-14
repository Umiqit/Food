using FoodServiceOccupancyForecast.Core.Interfaces;
using FoodServiceOccupancyForecast.Core.Services;
using FoodServiceOccupancyForecast.VideoAnalysis.Processing;
using FoodServiceOccupancyForecast.Web.Hubs;
using FoodServiceOccupancyForecast.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddControllers();
builder.Services.AddSignalR();

builder.Services.AddSingleton<InMemoryRestaurantStore>();
builder.Services.AddSingleton<ITableRepository, InMemoryTableRepository>();
builder.Services.AddSingleton<IBookingRepository, InMemoryBookingRepository>();
builder.Services.AddScoped<IOccupancyService, OccupancyService>();
builder.Services.AddScoped<BookingService>();
builder.Services.AddScoped<TableService>();
builder.Services.AddSingleton<VideoProcessingService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
    app.UseHttpsRedirection();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapHub<OccupancyHub>("/occupancyHub");
app.MapHub<TableHub>("/tableHub");

app.Run();
