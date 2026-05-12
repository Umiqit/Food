// Floor plan interactions
class FloorPlanManager {
    constructor(containerId) {
        this.container = document.getElementById(containerId);
        this.tables = [];
        this.tooltip = this.createTooltip();
        this.init();
    }

    createTooltip() {
        const tooltip = document.createElement('div');
        tooltip.className = 'tooltip';
        document.body.appendChild(tooltip);
        return tooltip;
    }

    init() {
        this.loadTables();
        this.setupInteractions();
    }

    async loadTables() {
        try {
            const response = await fetch('/api/tables');
            this.tables = await response.json();
            this.renderTables();
        } catch (err) {
            console.error('Failed to load tables:', err);
        }
    }

    renderTables() {
        // Tables are rendered server-side, this handles dynamic updates
    }

    setupInteractions() {
        // Tooltip on hover
        this.container.querySelectorAll('.table, .admin-table, .client-table').forEach(table => {
            table.addEventListener('mouseenter', (e) => {
                const id = table.dataset.id;
                const status = table.dataset.status;
                const seats = table.dataset.seats || '?';

                this.tooltip.innerHTML = `
                    <strong>Стол ${id}</strong><br>
                    Статус: ${this.translateStatus(status)}<br>
                    Мест: ${seats}
                `;
                this.tooltip.style.opacity = '1';
            });

            table.addEventListener('mousemove', (e) => {
                this.tooltip.style.left = (e.pageX + 15) + 'px';
                this.tooltip.style.top = (e.pageY - 10) + 'px';
            });

            table.addEventListener('mouseleave', () => {
                this.tooltip.style.opacity = '0';
            });
        });
    }

    translateStatus(status) {
        const map = {
            'Free': 'Свободен',
            'Occupied': 'Занят',
            'Reserved': 'Забронирован',
            'Cleaning': 'Уборка'
        };
        return map[status] || status;
    }

    async updateTableStatus(id, status, guests = null) {
        try {
            const body = { status: status };
            if (guests !== null) body.guests = guests;

            await fetch(`/api/tables/${id}/status`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(body)
            });
        } catch (err) {
            console.error('Failed to update status:', err);
        }
    }

    async updateTablePosition(id, x, y) {
        try {
            await fetch(`/api/tables/${id}/position`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ x, y })
            });
        } catch (err) {
            console.error('Failed to update position:', err);
        }
    }
}

// Drag and drop for admin
function initDragAndDrop() {
    let draggedEl = null;
    let offsetX = 0;
    let offsetY = 0;

    document.querySelectorAll('.admin-table').forEach(table => {
        table.addEventListener('dragstart', (e) => {
            draggedEl = table;
            offsetX = e.offsetX;
            offsetY = e.offsetY;
            table.style.opacity = '0.5';
            e.dataTransfer.effectAllowed = 'move';
        });

        table.addEventListener('dragend', async (e) => {
            table.style.opacity = '1';
            draggedEl = null;
        });
    });

    const container = document.getElementById('admin-plan');
    if (container) {
        container.addEventListener('dragover', (e) => {
            e.preventDefault();
            e.dataTransfer.dropEffect = 'move';
        });

        container.addEventListener('drop', async (e) => {
            e.preventDefault();
            if (!draggedEl) return;

            const rect = container.getBoundingClientRect();
            const x = e.clientX - rect.left - offsetX;
            const y = e.clientY - rect.top - offsetY;

            draggedEl.style.left = x + 'px';
            draggedEl.style.top = y + 'px';

            const id = draggedEl.dataset.id;
            await fetch(`/api/tables/${id}/position`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ x, y })
            });
        });
    }
}

// Initialize
document.addEventListener('DOMContentLoaded', () => {
    if (document.getElementById('admin-plan')) {
        initDragAndDrop();
    }
});
