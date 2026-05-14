/**
 * Restaurant Map Interactive Controller
 * FoodServiceOccupancyForecast
 * Fixed version: overlay bug, separate pages, auto-scale, active nav highlighting
 */

class RestaurantMap {
    constructor() {
        this.currentHall = 'general';
        this.tableStatuses = {};
        this.selectedTable = null;
        this.init();
    }

    init() {
        this.loadTableStatuses();
        this.bindEvents();
        this.updateStatistics();
        this.scaleMap();
        window.addEventListener('resize', () => this.scaleMap());

        // FIX #2: Start with empty map, show only selected hall
        this.switchHall('general');
    }

    // FIX #3: Auto-scale map to fit viewport
    scaleMap() {
        const map = document.querySelector('.restaurant-map');
        if (!map) return;

        const viewportW = window.innerWidth;
        const viewportH = window.innerHeight;
        const mapW = 1920;
        const mapH = 1080;

        const scaleX = viewportW / mapW;
        const scaleY = viewportH / mapH;
        const scale = Math.min(scaleX, scaleY);

        map.style.transform = `scale(${scale})`;
        map.style.transformOrigin = 'top left';

        // Center the map
        const offsetX = (viewportW - mapW * scale) / 2;
        const offsetY = (viewportH - mapH * scale) / 2;
        map.style.marginLeft = `${Math.max(0, offsetX)}px`;
        map.style.marginTop = `${Math.max(0, offsetY)}px`;
    }

    // Load table statuses from API/localStorage
    loadTableStatuses() {
        const defaultStatuses = {
            1: 'reserved', 2: 'free', 3: 'free', 4: 'free', 5: 'free',
            6: 'occupied', 7: 'free', 8: 'reserved', 9: 'occupied', 10: 'free', 11: 'occupied',
            13: 'free', 14: 'free', 15: 'occupied', 16: 'free', 17: 'reserved', 18: 'occupied',
            19: 'occupied', 20: 'free', 21: 'free',
            24: 'free', 25: 'free', 26: 'free', 27: 'free', 28: 'free', 29: 'free'
        };

        const saved = localStorage.getItem('tableStatuses');
        this.tableStatuses = saved ? JSON.parse(saved) : defaultStatuses;
        this.applyTableStatuses();
    }

    applyTableStatuses() {
        document.querySelectorAll('.table-group').forEach(group => {
            const numberEl = group.querySelector('.table__number');
            const bodyEl = group.querySelector('.table__body');
            if (!numberEl || !bodyEl) return;

            const tableNum = numberEl.textContent.trim();
            const status = this.tableStatuses[tableNum] || 'free';

            bodyEl.classList.remove('table__body--free', 'table__body--occupied', 'table__body--reserved');
            bodyEl.classList.add(`table__body--${status}`);
            group.dataset.status = status;
            group.dataset.tableNumber = tableNum;
        });
    }

    saveTableStatuses() {
        localStorage.setItem('tableStatuses', JSON.stringify(this.tableStatuses));
    }

    bindEvents() {
        // Table clicks
        document.querySelectorAll('.table-group').forEach(table => {
            table.addEventListener('click', (e) => this.handleTableClick(e, table));
            table.addEventListener('mouseenter', (e) => this.showTableTooltip(e, table));
            table.addEventListener('mouseleave', () => this.hideTableTooltip());
        });

        // Navigation - hall buttons
        document.querySelectorAll('.nav-item').forEach(item => {
            item.addEventListener('click', (e) => this.handleNavigation(e, item));
        });

        // Sub buttons (popups)
        document.querySelector('.contain-buttons__personal')?.addEventListener('click', () => this.openPopup('personal'));
        document.querySelector('.contain-buttons__analitics')?.addEventListener('click', () => this.openPopup('analytics'));
        document.querySelector('.contain-buttons__reservations')?.addEventListener('click', () => this.openPopup('reserve'));
        document.querySelector('.contain-buttons__info-about')?.addEventListener('click', () => this.openPopup('information'));

        // Popup close
        document.querySelectorAll('.popup-overlay').forEach(overlay => {
            overlay.addEventListener('click', (e) => {
                if (e.target === overlay) this.closePopup(overlay.id);
            });
        });

        document.querySelectorAll('.popup-close').forEach(btn => {
            btn.addEventListener('click', () => {
                const overlay = btn.closest('.popup-overlay');
                if (overlay) overlay.classList.remove('active');
            });
        });
    }

    handleTableClick(e, table) {
        e.stopPropagation();
        const tableNum = table.dataset.tableNumber;
        const currentStatus = table.dataset.status || 'free';

        this.selectedTable = tableNum;

        const statusCycle = { free: 'reserved', reserved: 'occupied', occupied: 'free' };
        const newStatus = statusCycle[currentStatus] || 'free';

        this.tableStatuses[tableNum] = newStatus;
        this.saveTableStatuses();
        this.applyTableStatuses();
        this.updateStatistics();

        this.showStatusChangeAnimation(table, newStatus);
    }

    showTableTooltip(e, table) {
        const tooltip = document.getElementById('table-tooltip');
        if (!tooltip) return;

        const tableNum = table.dataset.tableNumber;
        const status = table.dataset.status || 'free';
        const statusText = { free: 'Свободен', occupied: 'Занят', reserved: 'Забронирован' };

        tooltip.querySelector('.tooltip-table-number').textContent = `Стол № ${tableNum}`;
        tooltip.querySelector('.tooltip-status').textContent = `Статус: ${statusText[status]}`;
        tooltip.querySelector('.tooltip-guests').textContent = 'Количество гостей: 0';
        tooltip.querySelector('.tooltip-waiter').textContent = 'Официант: №0';

        const rect = table.getBoundingClientRect();
        tooltip.style.left = `${rect.right + 10}px`;
        tooltip.style.top = `${rect.top}px`;
        tooltip.classList.add('active');
    }

    hideTableTooltip() {
        const tooltip = document.getElementById('table-tooltip');
        if (tooltip) tooltip.classList.remove('active');
    }

    showStatusChangeAnimation(table, status) {
        const colors = { free: '#016802', occupied: '#FF0000', reserved: '#AA00FF' };
        const body = table.querySelector('.table__body');

        body.style.transition = 'all 0.3s ease';
        body.style.boxShadow = `0 0 20px ${colors[status]}`;

        setTimeout(() => {
            body.style.boxShadow = 'none';
        }, 500);
    }

    // FIX #4: Navigation with active button highlighting
    handleNavigation(e, item) {
        const hall = item.dataset.hall;
        if (!hall) return;

        // Update active state in sidebar
        document.querySelectorAll('.nav-item').forEach(n => {
            n.classList.remove('nav-item--active', 'nav-item--highlight');
        });
        item.classList.add('nav-item--active');

        // Add highlight class for visual feedback
        if (hall === 'hall3') {
            item.classList.add('nav-item--highlight');
        }

        this.currentHall = hall;
        this.switchHall(hall);

        // Update URL hash for "separate page" feel
        window.location.hash = hall;
    }

    // FIX #1 & #2: Proper hall switching - hide all, show only selected
    switchHall(hall) {
        const hallSelectors = {
            general: ['.hall-1-container', '.circle-hall-container', '.hall-3-container', '.veranda-container', '.kitchen-container', '.bar-container', '.wardrobe-container', '.reception-container', '.corridor2', '.exite-block', '.WC1-container', '.WC2', '.WC3-container', '.kid-room-container', '.ladder-container', '.dishwasher-room-container'],
            hall1: ['.hall-1-container'],
            hall2: ['.circle-hall-container'],
            hall3: ['.hall-3-container'],
            veranda: ['.veranda-container']
        };

        // FIX #1: First, HIDE ALL hall elements completely (not just opacity)
        const allHalls = document.querySelectorAll('.hall-1-container, .circle-hall-container, .hall-3-container, .veranda-container, .kitchen-container, .bar-container, .wardrobe-container, .reception-container, .corridor2, .exite-block, .WC1-container, .WC2, .WC3-container, .kid-room-container, .ladder-container, .dishwasher-room-container');

        allHalls.forEach(h => {
            h.style.display = 'none';
            h.style.opacity = '0';
            h.style.pointerEvents = 'none';
        });

        // Show selected hall(s)
        const targets = hallSelectors[hall] || hallSelectors.general;

        targets.forEach(selector => {
            const elements = document.querySelectorAll(selector);
            elements.forEach(el => {
                el.style.display = 'block';
                el.style.opacity = '1';
                el.style.pointerEvents = 'auto';
            });
        });

        // Always show common elements (header, sidebar, background, prompt)
        const commonElements = document.querySelectorAll('.header, .left-menu-container, .vetka, .prompt');
        commonElements.forEach(el => {
            el.style.display = 'block';
            el.style.opacity = '1';
        });

        // Update statistics based on visible hall
        this.updateStatistics();
    }

    updateStatistics() {
        const statuses = Object.values(this.tableStatuses);
        const total = statuses.length;
        const occupied = statuses.filter(s => s === 'occupied').length;
        const reserved = statuses.filter(s => s === 'reserved').length;
        const free = statuses.filter(s => s === 'free').length;

        const stat1 = document.querySelector('.statistics-1-container .statistics-text');
        if (stat1) stat1.textContent = `Занято ${occupied} из ${total} столов`;

        const stat2 = document.querySelector('.statistics-2-container .statistics-text');
        if (stat2) stat2.textContent = `Количество гостей: ${occupied * 4}`;

        const stat3 = document.querySelector('.statistics-3-container .statistics-text');
        if (stat3) stat3.textContent = `Забронировано ${reserved} столов`;

        const loadIndicator = document.querySelector('.load-indicator');
        if (loadIndicator) {
            const loadPercent = total > 0 ? Math.round((occupied / total) * 100) : 0;
            loadIndicator.textContent = `Текущая нагрузка: ${loadPercent}%`;
        }
    }

    openPopup(type) {
        const popupIds = {
            personal: 'popup-personal',
            analytics: 'popup-analytics',
            reserve: 'popup-reserve',
            information: 'popup-information',
            status: 'popup-status'
        };

        const popupId = popupIds[type];
        if (popupId) {
            const popup = document.getElementById(popupId);
            if (popup) popup.classList.add('active');
        }
    }

    closePopup(popupId) {
        const popup = document.getElementById(popupId);
        if (popup) popup.classList.remove('active');
    }

    async fetchTableData() {
        try {
            const response = await fetch('/api/tables');
            const data = await response.json();
            this.tableStatuses = data;
            this.applyTableStatuses();
            this.updateStatistics();
        } catch (error) {
            console.error('Failed to fetch table data:', error);
        }
    }

    async updateTableStatus(tableId, status) {
        try {
            await fetch(`/api/tables/${tableId}/status`, {
                method: 'PUT',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ status })
            });
        } catch (error) {
            console.error('Failed to update table status:', error);
        }
    }
}

// Initialize on DOM ready
document.addEventListener('DOMContentLoaded', () => {
    window.restaurantMap = new RestaurantMap();

    // Handle URL hash for direct navigation
    const hash = window.location.hash.replace('#', '');
    if (hash && ['general', 'hall1', 'hall2', 'hall3', 'veranda'].includes(hash)) {
        setTimeout(() => {
            const navItem = document.querySelector(`.nav-item[data-hall="${hash}"]`);
            if (navItem) navItem.click();
        }, 100);
    }
});

// SignalR connection for real-time updates
class RealTimeUpdates {
    constructor() {
        this.connection = null;
        this.init();
    }

    async init() {
        // SignalR hub connection placeholder
    }
}

if (typeof module !== 'undefined' && module.exports) {
    module.exports = { RestaurantMap, RealTimeUpdates };
}
