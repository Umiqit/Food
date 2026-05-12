// SignalR client connection
const connection = new signalr.HubConnectionBuilder()
    .withUrl("/occupancyHub")
    .withAutomaticReconnect()
    .build();

connection.on("OccupancyUpdated", (snapshot) => {
    console.log("Occupancy updated:", snapshot);

    // Update stats if elements exist
    const occupiedEl = document.getElementById('occupied-tables');
    const guestsEl = document.getElementById('total-guests');
    const reservedEl = document.getElementById('reserved-tables');
    const loadEl = document.getElementById('load-percentage');

    if (occupiedEl) occupiedEl.textContent = snapshot.occupiedTables;
    if (guestsEl) guestsEl.textContent = snapshot.totalGuests;
    if (reservedEl) reservedEl.textContent = snapshot.reservedTables;
    if (loadEl) loadEl.textContent = snapshot.loadPercentage.toFixed(1) + '%';
});

connection.on("TableStatusChanged", (data) => {
    console.log("Table status changed:", data);

    // Update table visual
    const table = document.querySelector(`[data-id="${data.tableId}"]`);
    if (table) {
        table.classList.remove('table-free', 'table-occupied', 'table-reserved');
        table.classList.add(`table-${data.status.toLowerCase()}`);
        table.dataset.status = data.status;

        // Update guest count display if exists
        const seatsEl = table.querySelector('.table-seats');
        if (seatsEl && data.guests > 0) {
            seatsEl.textContent = `${data.guests} гостей`;
        }
    }
});

connection.on("NewBooking", (booking) => {
    console.log("New booking:", booking);

    // Show notification
    if ('Notification' in window && Notification.permission === 'granted') {
        new Notification('Новое бронирование', {
            body: `Стол ${booking.tableId} — ${booking.customerName}`,
            icon: '/favicon.ico'
        });
    }

    // Refresh page if on bookings page
    if (window.location.pathname.includes('Bookings')) {
        location.reload();
    }
});

async function startConnection() {
    try {
        await connection.start();
        console.log("SignalR connected");

        // Request notification permission
        if ('Notification' in window && Notification.permission === 'default') {
            Notification.requestPermission();
        }
    } catch (err) {
        console.error("SignalR connection error:", err);
        setTimeout(startConnection, 5000);
    }
}

startConnection();
