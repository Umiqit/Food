using Microsoft.EntityFrameworkCore;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<Table> Tables => Set<Table>();
    public DbSet<Booking> Bookings => Set<Booking>();
    public DbSet<Visitor> Visitors => Set<Visitor>();
    public DbSet<OccupancySnapshot> OccupancySnapshots => Set<OccupancySnapshot>();
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Table>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.HallName).HasMaxLength(100);
            entity.Property(e => e.Shape).HasMaxLength(20);
            entity.HasIndex(e => e.Status);
            entity.HasIndex(e => e.HallName);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CustomerName).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CustomerPhone).HasMaxLength(20);
            entity.Property(e => e.CustomerEmail).HasMaxLength(200);
            entity.HasIndex(e => e.BookingDate);
            entity.HasIndex(e => e.Status);
            entity.HasOne(e => e.Table)
                  .WithMany(t => t.Bookings)
                  .HasForeignKey(e => e.TableId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Visitor>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Timestamp);
        });

        modelBuilder.Entity<OccupancySnapshot>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Timestamp);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Username).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(e => e.Username).IsUnique();
            entity.HasIndex(e => e.Email).IsUnique();
        });

        // Seed data
        modelBuilder.Entity<Table>().HasData(
            new Table { Id = 1, Name = "Стол 1", Seats = 2, Status = Core.Enums.TableStatus.Reserved, PositionX = 580, PositionY = 80, Shape = "rect", HallName = "Основной зал" },
            new Table { Id = 2, Name = "Стол 2", Seats = 4, Status = Core.Enums.TableStatus.Free, PositionX = 650, PositionY = 120, Shape = "square", HallName = "Основной зал" },
            new Table { Id = 3, Name = "Стол 3", Seats = 2, Status = Core.Enums.TableStatus.Free, PositionX = 760, PositionY = 80, Shape = "rect", HallName = "Основной зал" },
            new Table { Id = 4, Name = "Стол 4", Seats = 2, Status = Core.Enums.TableStatus.Free, PositionX = 620, PositionY = 180, Shape = "rect", HallName = "Основной зал" },
            new Table { Id = 5, Name = "Стол 5", Seats = 4, Status = Core.Enums.TableStatus.Free, PositionX = 520, PositionY = 220, Shape = "square", HallName = "Основной зал" },
            new Table { Id = 6, Name = "Стол 6", Seats = 2, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 4, PositionX = 580, PositionY = 300, Shape = "rect", HallName = "Основной зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-30) },
            new Table { Id = 7, Name = "Стол 7", Seats = 2, Status = Core.Enums.TableStatus.Free, PositionX = 660, PositionY = 300, Shape = "rect", HallName = "Основной зал" },
            new Table { Id = 8, Name = "Стол 8", Seats = 2, Status = Core.Enums.TableStatus.Reserved, PositionX = 780, PositionY = 220, Shape = "rect", HallName = "Основной зал" },
            new Table { Id = 9, Name = "Стол 9", Seats = 4, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 2, PositionX = 520, PositionY = 360, Shape = "square", HallName = "Основной зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-15) },
            new Table { Id = 10, Name = "Стол 10", Seats = 2, Status = Core.Enums.TableStatus.Free, PositionX = 600, PositionY = 420, Shape = "rect", HallName = "Основной зал" },
            new Table { Id = 11, Name = "Стол 11", Seats = 2, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 6, PositionX = 760, PositionY = 360, Shape = "rect", HallName = "Основной зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-45) },
            new Table { Id = 12, Name = "Стол 12", Seats = 2, Status = Core.Enums.TableStatus.Free, PositionX = 580, PositionY = 620, Shape = "rect", HallName = "Круглый зал" },
            new Table { Id = 13, Name = "Стол 13", Seats = 4, Status = Core.Enums.TableStatus.Free, PositionX = 650, PositionY = 600, Shape = "round", HallName = "Круглый зал" },
            new Table { Id = 14, Name = "Стол 14", Seats = 4, Status = Core.Enums.TableStatus.Free, PositionX = 720, PositionY = 600, Shape = "round", HallName = "Круглый зал" },
            new Table { Id = 15, Name = "Стол 15", Seats = 2, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 3, PositionX = 820, PositionY = 620, Shape = "rect", HallName = "Круглый зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-20) },
            new Table { Id = 16, Name = "Стол 16", Seats = 4, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 2, PositionX = 620, PositionY = 720, Shape = "square", HallName = "Круглый зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-10) },
            new Table { Id = 17, Name = "Стол 17", Seats = 2, Status = Core.Enums.TableStatus.Reserved, PositionX = 720, PositionY = 740, Shape = "rect", HallName = "Круглый зал" },
            new Table { Id = 18, Name = "Стол 18", Seats = 4, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 4, PositionX = 820, PositionY = 680, Shape = "square", HallName = "Круглый зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-25) },
            new Table { Id = 19, Name = "Стол 19", Seats = 4, Status = Core.Enums.TableStatus.Occupied, CurrentGuests = 2, PositionX = 900, PositionY = 480, Shape = "square", HallName = "Боковой зал", OccupiedSince = DateTime.UtcNow.AddMinutes(-35) },
            new Table { Id = 20, Name = "Стол 20", Seats = 2, Status = Core.Enums.TableStatus.Free, PositionX = 950, PositionY = 540, Shape = "rect", HallName = "Боковой зал" },
            new Table { Id = 21, Name = "Стол 21", Seats = 4, Status = Core.Enums.TableStatus.Free, PositionX = 920, PositionY = 600, Shape = "round", HallName = "Боковой зал" }
        );

        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Username = "admin", Email = "admin@gulinov.ru", PasswordHash = "AQAAAAEAACcQAAAAE...", Role = Core.Enums.UserRole.Admin },
            new User { Id = 2, Username = "manager", Email = "manager@gulinov.ru", PasswordHash = "AQAAAAEAACcQAAAAE...", Role = Core.Enums.UserRole.Manager }
        );
    }
}
