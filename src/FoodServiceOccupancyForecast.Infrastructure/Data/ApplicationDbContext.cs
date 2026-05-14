using Microsoft.EntityFrameworkCore;
using FoodServiceOccupancyForecast.Core.Entities;

namespace FoodServiceOccupancyForecast.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Table> Tables { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Visitor> Visitors { get; set; }
        public DbSet<Hall> Halls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Table>().HasKey(t => t.Id);
            modelBuilder.Entity<Table>().Property(t => t.Number).IsRequired();
            modelBuilder.Entity<Table>().Property(t => t.Status).HasConversion<string>();

            modelBuilder.Entity<Booking>().HasKey(b => b.Id);
            modelBuilder.Entity<Booking>().Property(b => b.Status).HasConversion<string>();

            // Seed data for halls
            modelBuilder.Entity<Hall>().HasData(
                new Hall { Id = 1, Name = "Main hall", Description = "Main dining hall", Capacity = 11 },
                new Hall { Id = 2, Name = "Circle Hall", Description = "Circle shaped hall", Capacity = 6 },
                new Hall { Id = 3, Name = "2nd floor", Description = "Second floor hall", Capacity = 6 },
                new Hall { Id = 4, Name = "Veranda", Description = "Outdoor veranda", Capacity = 3 }
            );

            // Seed data for restaurant map
            modelBuilder.Entity<Table>().HasData(
                // Hall 1 (Main hall)
                new Table { Id = 1, Number = 1, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Reserved, PositionX = 26.19, PositionY = 22, Width = 45.99, Height = 89.84, Rotation = -0.11, Shape = "rectangle" },
                new Table { Id = 2, Number = 2, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 174, PositionY = 49, Width = 62, Height = 61.95, Rotation = 46.23, Shape = "square" },
                new Table { Id = 3, Number = 3, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 296.67, PositionY = 25, Width = 45.99, Height = 89.84, Rotation = -0.11, Shape = "rectangle" },
                new Table { Id = 4, Number = 4, HallId = 1, Capacity = 6, Status = Core.Enums.TableStatus.Free, PositionX = 133.55, PositionY = 143.96, Width = 90.2, Height = 46.7, Rotation = -90.56, Shape = "rectangle" },
                new Table { Id = 5, Number = 5, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 17, PositionY = 197, Width = 62, Height = 61.95, Rotation = 46.23, Shape = "square" },
                new Table { Id = 6, Number = 6, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Occupied, PositionX = 99.64, PositionY = 274, Width = 45.81, Height = 89.75, Rotation = -0.11, Shape = "rectangle" },
                new Table { Id = 7, Number = 7, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 210.96, PositionY = 273, Width = 45.81, Height = 89.75, Rotation = -0.11, Shape = "rectangle" },
                new Table { Id = 8, Number = 8, HallId = 1, Capacity = 6, Status = Core.Enums.TableStatus.Reserved, PositionX = 339, PositionY = 238, Width = 90.2, Height = 46.7, Rotation = -90.56, Shape = "rectangle" },
                new Table { Id = 9, Number = 9, HallId = 1, Capacity = 4, Status = Core.Enums.TableStatus.Occupied, PositionX = 13, PositionY = 390, Width = 62, Height = 61.95, Rotation = 46.23, Shape = "square" },
                new Table { Id = 10, Number = 10, HallId = 1, Capacity = 6, Status = Core.Enums.TableStatus.Free, PositionX = 139.8, PositionY = 444.9, Width = 90.2, Height = 46.7, Rotation = -90.56, Shape = "rectangle" },
                new Table { Id = 11, Number = 11, HallId = 1, Capacity = 6, Status = Core.Enums.TableStatus.Occupied, PositionX = 339, PositionY = 411, Width = 90.2, Height = 46.7, Rotation = -90.56, Shape = "rectangle" },
                // Circle Hall
                new Table { Id = 13, Number = 13, HallId = 2, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 89, PositionY = 143, Width = 53.06, Height = 53.08, Rotation = -43.89, Shape = "square" },
                new Table { Id = 14, Number = 14, HallId = 2, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 172, PositionY = 142, Width = 54.51, Height = 54.51, Rotation = -44.95, Shape = "square" },
                new Table { Id = 15, Number = 15, HallId = 2, Capacity = 4, Status = Core.Enums.TableStatus.Occupied, PositionX = 269.95, PositionY = 86.8, Width = 72.59, Height = 58.24, Rotation = -112, Shape = "rectangle" },
                new Table { Id = 16, Number = 16, HallId = 2, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 6, PositionY = 167.09, Width = 69.14, Height = 38, Rotation = -89.82, Shape = "rectangle" },
                new Table { Id = 17, Number = 17, HallId = 2, Capacity = 4, Status = Core.Enums.TableStatus.Reserved, PositionX = 160, PositionY = 264, Width = 69.27, Height = 35.54, Rotation = -89.98, Shape = "rectangle" },
                new Table { Id = 18, Number = 18, HallId = 2, Capacity = 4, Status = Core.Enums.TableStatus.Occupied, PositionX = 253, PositionY = 215, Width = 72.8, Height = 64.37, Rotation = -57.47, Shape = "rectangle" },
                // Veranda
                new Table { Id = 19, Number = 19, HallId = 4, Capacity = 4, Status = Core.Enums.TableStatus.Occupied, PositionX = 6, PositionY = 29, Width = 54.51, Height = 54.51, Rotation = -44.95, Shape = "square" },
                new Table { Id = 20, Number = 20, HallId = 4, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 79, PositionY = 87.72, Width = 39.8, Height = 39.09, Rotation = -91.28, Shape = "square" },
                new Table { Id = 21, Number = 21, HallId = 4, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 9, PositionY = 161, Width = 54.51, Height = 54.51, Rotation = -44.95, Shape = "square" },
                // Hall 3 (2nd floor)
                new Table { Id = 24, Number = 24, HallId = 3, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 643, PositionY = 53, Width = 40.8, Height = 40.89, Rotation = -89.82, Shape = "square" },
                new Table { Id = 25, Number = 25, HallId = 3, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 508, PositionY = 39, Width = 75.28, Height = 41, Rotation = -89.82, Shape = "rectangle" },
                new Table { Id = 26, Number = 26, HallId = 3, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 664.59, PositionY = 365, Width = 41.37, Height = 75.48, Rotation = -0.46, Shape = "rectangle" },
                new Table { Id = 27, Number = 27, HallId = 3, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 643, PositionY = 476, Width = 40.8, Height = 40.89, Rotation = -89.82, Shape = "square" },
                new Table { Id = 28, Number = 28, HallId = 3, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 508, PositionY = 484, Width = 75.28, Height = 41, Rotation = -89.82, Shape = "rectangle" },
                new Table { Id = 29, Number = 29, HallId = 3, Capacity = 4, Status = Core.Enums.TableStatus.Free, PositionX = 351, PositionY = 484, Width = 75.28, Height = 41, Rotation = -89.82, Shape = "rectangle" }
            );
        }
    }
}
