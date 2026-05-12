# FoodServiceOccupancyForecast

## Система прогнозирования загруженности заведений общественного питания

**Ресторан "Гулиновъ"** — мониторинг столов, бронирование, AI-видеоаналитика.

---

## 🏗 Архитектура

```
src/
├── FoodServiceOccupancyForecast.Core/          # Сущности, интерфейсы, бизнес-логика
├── FoodServiceOccupancyForecast.Infrastructure/ # EF Core, PostgreSQL, репозитории
├── FoodServiceOccupancyForecast.VideoAnalysis/ # AI-детекция людей, камеры
└── FoodServiceOccupancyForecast.Web/           # API + Razor Pages + SignalR
```

---

## 🚀 Быстрый старт

### 1. Требования
- .NET 8 SDK
- PostgreSQL 14+
- Visual Studio 2022 или VS Code

### 2. Настройка БД
```bash
# appsettings.json
"ConnectionStrings": {
  "DefaultConnection": "Host=localhost;Database=FoodServiceOccupancy;Username=postgres;Password=your_password"
}
```

### 3. Миграции
```bash
cd src/FoodServiceOccupancyForecast.Infrastructure
dotnet ef migrations add InitialCreate --startup-project ../FoodServiceOccupancyForecast.Web
dotnet ef database update --startup-project ../FoodServiceOccupancyForecast.Web
```

### 4. Запуск
```bash
cd src/FoodServiceOccupancyForecast.Web
dotnet run
```

- API: https://localhost:5001/api/
- Swagger: https://localhost:5001/swagger
- Admin: https://localhost:5001/Admin/Dashboard
- Client: https://localhost:5001/Client/Index
- SignalR: wss://localhost:5001/occupancyHub

---

## 📡 API Endpoints

### Occupancy
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/occupancy/current` | Текущая загрузка |
| GET | `/api/occupancy/peak` | Пиковые часы |
| GET | `/api/occupancy/forecast` | Прогноз загрузки |
| GET | `/api/occupancy/load` | Процент загрузки |

### Tables
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/tables` | Все столы |
| GET | `/api/tables/available` | Свободные столы |
| GET | `/api/tables/{id}` | Стол по ID |
| PUT | `/api/tables/{id}/status` | Изменить статус |
| PUT | `/api/tables/{id}/position` | Переместить стол |
| POST | `/api/tables` | Создать стол |
| DELETE | `/api/tables/{id}` | Удалить стол |

### Bookings
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/bookings` | Все брони |
| GET | `/api/bookings/pending` | Ожидающие |
| POST | `/api/bookings` | Создать бронь |
| PUT | `/api/bookings/{id}/confirm` | Подтвердить |
| DELETE | `/api/bookings/{id}` | Отменить |

### Analytics
| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/analytics/daily` | Дневной отчёт |
| GET | `/api/analytics/weekly` | Недельный |
| GET | `/api/analytics/monthly` | Месячный |

---

## 📡 SignalR Events

```javascript
connection.on("OccupancyUpdated", (snapshot) => {
    // { totalGuests, occupiedTables, reservedTables, freeTables, loadPercentage }
});

connection.on("TableStatusChanged", (data) => {
    // { tableId, status, guests }
});

connection.on("NewBooking", (booking) => {
    // { id, tableId, customerName, status }
});
```

---

## 🎨 Страницы

| Page | URL | Доступ |
|------|-----|--------|
| Dashboard | `/Admin/Dashboard` | Admin, Manager |
| Tables | `/Admin/Tables` | Admin, Manager |
| Bookings | `/Admin/Bookings` | Admin, Manager |
| Reports | `/Admin/Reports` | Admin, Manager |
| Client Booking | `/Client/Index` | Public |

---

## 📋 Чеклист реализации

### ✅ Этап 1 — Настройка проекта
- [x] CORS настроен
- [x] Swagger включён
- [x] SignalR хаб зарегистрирован
- [x] Зависимости между проектами
- [x] Program.cs конфигурация

### ✅ Этап 2 — API Контроллеры
- [x] OccupancyController (current, peak, forecast)
- [x] TablesController (CRUD + status + position)
- [x] BookingsController (create, confirm, cancel)
- [x] AnalyticsController (daily, weekly, monthly)

### ✅ Этап 3 — SignalR
- [x] OccupancyHub
- [x] BroadcastOccupancyUpdate
- [x] BroadcastTableStatusChange
- [x] BroadcastNewBooking

### ✅ Этап 4 — Админка (Razor Pages)
- [x] Dashboard с графиками (Chart.js)
- [x] Tables с drag-and-drop
- [x] Bookings с фильтрами
- [x] Reports (PDF/Excel stubs)

### ✅ Этап 5 — Видеоаналитика
- [x] PeopleDetector (mock AI)
- [x] CameraClient (RTSP stub)
- [x] VideoProcessingService
- [x] OccupancyBackgroundService (10s interval)

### ✅ Этап 6 — Аутентификация
- [x] Cookie auth
- [x] Policy-based authorization
- [x] Roles: Admin, Manager, Observer

---

## 🧪 Тесты

```bash
dotnet test
```

---

## 📝 Примечания

- **Seed data**: 21 стол с реальными позициями плана зала "Гулиновъ"
- **Mock AI**: PeopleDetector возвращает случайные значения (заменить на ONNX/YOLO)
- **Camera**: CameraClient — stub (заменить на FFmpeg/OpenCV)
- **Reports**: stubs для PDF/Excel (добавить iTextSharp/EPPlus)

---

## 👥 Команда

| Участник | Роль | Зона |
|----------|------|------|
| @nikolaikorentsov | Бэкенд | Core, бизнес-логика |
| Umi | Бэкенд | Web, VideoAnalysis, SignalR |
| Баркалов Илья | База данных | Infrastructure, миграции |
| White-Guse | Фронтенд | UI, верстка |

---

**© 2026 Гулиновъ — Система мониторинга загруженности**
