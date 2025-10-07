// ============================================
// wwwroot/admin/js/admin-app.js
// ============================================

// Simple SPA Router para el admin
class AdminApp {
    constructor() {
        this.currentView = 'dashboard';
        this.apiBase = '/api';
        this.init();
    }

    init() {
        this.loadView(this.currentView);
    }

    async loadView(view) {
        const appElement = document.getElementById('app');
        
        // Actualizar navegación activa
        document.querySelectorAll('.admin-nav a').forEach(a => a.classList.remove('active'));
        document.querySelector(`[onclick="navigateTo('${view}')"]`)?.classList.add('active');
        
        switch(view) {
            case 'dashboard':
                appElement.innerHTML = await this.renderDashboard();
                break;
            case 'hoteles':
                appElement.innerHTML = await window.HotelesModule.render();
                break;
            case 'habitaciones':
                appElement.innerHTML = await window.HabitacionesModule.render();
                break;
            case 'reservas':
                appElement.innerHTML = await window.ReservasModule.render();
                break;
            case 'usuarios':
                appElement.innerHTML = await window.UsuariosModule.render();
                break;
            case 'financiero':
                appElement.innerHTML = this.renderFinanciero();
                break;
            default:
                appElement.innerHTML = '<div class="card"><div class="card-body"><h2>Vista no encontrada</h2></div></div>';
        }
    }

    async renderDashboard() {
        try {
            // Obtener estadísticas básicas
            const hotelesResponse = await fetch(`${this.apiBase}/hotelesapi`);
            const hotelesData = hotelesResponse.ok ? await hotelesResponse.json() : null;
            // Manejar tanto 'data' como 'Data' (PascalCase vs camelCase)
            const hoteles = hotelesData?.data || hotelesData?.Data || hotelesData || [];

            const habitacionesResponse = await fetch(`${this.apiBase}/habitacionesapi`);
            const habitacionesData = habitacionesResponse.ok ? await habitacionesResponse.json() : null;
            const habitaciones = habitacionesData?.data || habitacionesData?.Data || habitacionesData || [];

            return `
                <section class="welcome-panel">
                    <div class="welcome-greeting">
                        <div class="greet-line">Hola, <span class="welcome-name">Admin</span></div>
                        <div class="greet-sub">Que tengas un bonito día</div>
                    </div>
                    <div class="page-info">
                        <h1 class="panel-title">Dashboard General</h1>
                        <div class="toolbar-inline">
                            <input type="search" placeholder="Buscar en el sistema..." aria-label="Buscar" />
                            <button class="btn btn-secondary">
                                <i class="fas fa-chart-bar"></i> Ver Reportes
                            </button>
                            <button class="btn btn-success">
                                <i class="fas fa-sync-alt"></i> Actualizar Datos
                            </button>
                        </div>
                    </div>
                </section>

                <div class="dashboard-grid">
                    <div class="metric-card">
                        <h3>Total Hoteles</h3>
                        <div class="metric blue">${Array.isArray(hoteles) ? hoteles.length : 0}</div>
                    </div>
                    <div class="metric-card">
                        <h3>Total Habitaciones</h3>
                        <div class="metric red">${Array.isArray(habitaciones) ? habitaciones.length : 0}</div>
                    </div>
                    <div class="metric-card">
                        <h3>Reservas Activas</h3>
                        <div class="metric green">-</div>
                    </div>
                    <div class="metric-card">
                        <h3>Total Usuarios</h3>
                        <div class="metric orange">-</div>
                    </div>
                </div>
                
                <div class="card">
                    <div class="card-header">
                        <h3>Accesos Rápidos</h3>
                    </div>
                    <div class="card-body">
                        <p>Gestiona todos los aspectos de tu cadena hotelera desde aquí:</p>
                        <br>
                        <div style="display: grid; grid-template-columns: repeat(auto-fit, minmax(200px, 1fr)); gap: 1rem;">
                            <button class="btn btn-success" onclick="navigateTo('hoteles')">
                                <i class="fas fa-hotel"></i> Gestionar Hoteles
                            </button>
                            <button class="btn btn-success" onclick="navigateTo('habitaciones')">
                                <i class="fas fa-bed"></i> Gestionar Habitaciones
                            </button>
                            <button class="btn btn-success" onclick="navigateTo('reservas')">
                                <i class="fas fa-calendar-check"></i> Ver Reservas
                            </button>
                            <button class="btn btn-success" onclick="navigateTo('usuarios')">
                                <i class="fas fa-users"></i> Gestionar Usuarios
                            </button>
                        </div>
                    </div>
                </div>
            `;
        } catch (error) {
            console.error('Error en dashboard:', error);
            return `
                <div class="card">
                    <div class="card-body">
                        <h1>Dashboard</h1>
                        <p>Error al cargar estadísticas: ${error.message}</p>
                    </div>
                </div>
            `;
        }
    }

    renderReservas() {
        return `
            <section class="welcome-panel">
                <div class="page-info">
                    <h1 class="panel-title">Gestión de Reservas</h1>
                    <div class="toolbar-inline">
                        <input type="search" placeholder="Buscar reservas por huésped o habitación..." aria-label="Buscar reservas" />
                        <button class="btn btn-secondary">
                            <i class="fas fa-calendar"></i> Filtrar por Fecha
                        </button>
                        <button class="btn btn-success">
                            <i class="fas fa-plus"></i> Nueva Reserva
                        </button>
                    </div>
                </div>
            </section>
            <div class="card">
                <div class="card-body">
                    <div style="text-align: center; padding: 3rem; color: #666;">
                        <i class="fas fa-calendar-check" style="font-size: 3rem; margin-bottom: 1rem; display: block; color: #3498db;"></i>
                        <h3>Módulo de Reservas</h3>
                        <p>Vista de reservas en desarrollo...</p>
                    </div>
                </div>
            </div>
        `;
    }

    renderFinanciero() {
        return `
            <section class="welcome-panel">
                <div class="page-info">
                    <h1 class="panel-title">Panel Financiero</h1>
                    <div class="toolbar-inline">
                        <input type="search" placeholder="Buscar transacciones o reportes..." aria-label="Buscar financiero" />
                        <button class="btn btn-secondary">
                            <i class="fas fa-chart-line"></i> Ver Gráficos
                        </button>
                        <button class="btn btn-success">
                            <i class="fas fa-file-export"></i> Exportar Reporte
                        </button>
                    </div>
                </div>
            </section>
            <div class="card">
                <div class="card-body">
                    <div style="text-align: center; padding: 3rem; color: #666;">
                        <i class="fas fa-dollar-sign" style="font-size: 3rem; margin-bottom: 1rem; display: block; color: #3498db;"></i>
                        <h3>Módulo Financiero</h3>
                        <p>Vista financiera en desarrollo...</p>
                    </div>
                </div>
            </div>
        `;
    }
}

// Función global para navegación
function navigateTo(view) {
    window.adminApp.currentView = view;
    window.adminApp.loadView(view);
}

// Función para mostrar alertas
function showAlert(message, type = 'success') {
    const alertContainer = document.getElementById('alert-container');
    const alertId = 'alert_' + Date.now();
    
    const alertHtml = `
        <div id="${alertId}" class="alert alert-${type} show">
            ${message}
        </div>
    `;
    
    alertContainer.innerHTML = alertHtml;
    
    setTimeout(() => {
        const alertElement = document.getElementById(alertId);
        if (alertElement) {
            alertElement.classList.remove('show');
            setTimeout(() => {
                alertContainer.innerHTML = '';
            }, 300);
        }
    }, 5000);
}

// Event listeners globales
window.onclick = function(event) {
    const hotelModal = document.getElementById('hotelModal');
    const habitacionModal = document.getElementById('habitacionModal');
    const usuarioModal = document.getElementById('usuarioModal');
    if (event.target === hotelModal && window.HotelesModule) {
        window.HotelesModule.closeModal();
    }
    if (event.target === habitacionModal && window.HabitacionesModule) {
        window.HabitacionesModule.closeModal();
    }
    if (event.target === usuarioModal && window.UsuariosModule) {
        window.UsuariosModule.closeModal();
    }
}

document.addEventListener('keydown', function(event) {
    if (event.key === 'Escape') {
        if (window.HotelesModule) window.HotelesModule.closeModal();
        if (window.HabitacionesModule) window.HabitacionesModule.closeModal();
        if (window.UsuariosModule) window.UsuariosModule.closeModal();
    }
});

// Inicializar la app
document.addEventListener('DOMContentLoaded', function() {
    window.adminApp = new AdminApp();
});