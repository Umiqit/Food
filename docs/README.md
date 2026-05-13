# FoodServiceOccupancyForecast

## Система прогнозирования загруженности заведений общественного питания

### 🚀 Быстрый старт

```bash
# 1. Установи .NET 8 SDK и PostgreSQL
# 2. Настрой строку подключения в appsettings.json
# 3. Запусти миграции
dotnet ef database update --project src/FoodServiceOccupancyForecast.Infrastructure

# 4. Запусти проект
dotnet run --project src/FoodServiceOccupancyForecast.Web
```

### 🗺️ Карта ресторана

Открой `/RestaurantMap` для просмотра интерактивной карты.

### 📡 API

- `GET /api/tables` — все столы
- `GET /api/tables/statistics` — статистика
- `PUT /api/tables/{id}/status` — обновить статус
- `GET /api/bookings` — все брони
- `GET /api/bookings/pending` — ожидающие брони

### 📡 SignalR

Хаб: `/tableHub`
- `TableStatusChanged` — изменение статуса стола
- `NewBookingReceived` — новая бронь
- `StaffAlert` — оповещение персоналу

### 🏗 Архитектура

```
Core → Infrastructure → Web
     ↘ VideoAnalysis ↗
```
