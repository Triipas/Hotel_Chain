// wwwroot/admin/js/admin-reservations.js

window.ReservasModule = {
    apiBase: '/api',
    currentReserva: null,

    async render() {
        try {
            console.log('Cargando reservas...');
            const response = await fetch(`${this.apiBase}/reservasapi`);
            
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }
            
            const responseData = await response.json();
            console.log('Respuesta de API:', responseData);
            
            const reservas = responseData?.data || responseData?.Data || responseData || [];
            
            console.log('Reservas procesadas:', reservas);
            
            if (!Array.isArray(reservas)) {
                console.error('Reservas no es un array:', reservas);
                throw new Error('La respuesta no es un array válido');
            }
            
            return `
                <section class="welcome-panel">
                    <div class="page-info">
                        <h1 class="panel-title">Gestión de Reservas</h1>
                        <div class="toolbar-inline">
                            <input type="search" id="searchReservas" placeholder="Buscar por número de reserva o usuario..." 
                                   aria-label="Buscar reservas" onkeyup="window.ReservasModule.search()" />
                            <select id="filterEstado" onchange="window.ReservasModule.search()">
                                <option value="">Todos los estados</option>
                                <option value="pendiente">Pendiente</option>
                                <option value="confirmada">Confirmada</option>
                                <option value="completada">Completada</option>
                                <option value="cancelada">Cancelada</option>
                            </select>
                            <select id="filterEstadoPago" onchange="window.ReservasModule.search()">
                                <option value="">Estados de pago</option>
                                <option value="pendiente">Pendiente</option>
                                <option value="pagado">Pagado</option>
                                <option value="reembolsado">Reembolsado</option>
                            </select>
                            <button class="btn btn-secondary" onclick="window.ReservasModule.exportarReporte()">
                                <i class="fas fa-download"></i> Exportar Reporte
                            </button>
                        </div>
                    </div>
                </section>
                
                <div class="table-container">
                    <div class="table-wrapper">
                        <table id="reservasTable">
                            <thead>
                                <tr>
                                    <th>Número Reserva</th>
                                    <th>Usuario</th>
                                    <th>Hotel</th>
                                    <th>Habitación</th>
                                    <th>Check-in</th>
                                    <th>Check-out</th>
                                    <th>Noches</th>
                                    <th>Precio Total</th>
                                    <th>Estado</th>
                                    <th>Pago</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${reservas.map(reserva => this.renderReservaRow(reserva)).join('')}
                            </tbody>
                        </table>
                        ${reservas.length === 0 ? '<div style="text-align: center; padding: 3rem; color: #666;"><i class="fas fa-calendar-check" style="font-size: 3rem; margin-bottom: 1rem; display: block;"></i>No hay reservas registradas</div>' : ''}
                    </div>
                </div>
            `;
        } catch (error) {
            console.error('Error al cargar reservas:', error);
            return `
                <div class="card">
                    <div class="card-body">
                        <h1>Error al cargar reservas</h1>
                        <p><strong>Error:</strong> ${error.message}</p>
                        <p>Verifica la consola del navegador para más detalles.</p>
                    </div>
                </div>
            `;
        }
    },

    renderReservaRow(reserva) {
        const reservaId = reserva.reservaId || reserva.ReservaId;
        const numeroReserva = reserva.numeroReserva || reserva.NumeroReserva;
        const usuario = reserva.usuario || reserva.Usuario;
        const habitacion = reserva.habitacion || reserva.Habitacion;
        const hotel = habitacion?.hotel || habitacion?.Hotel;
        const fechaInicio = reserva.fechaInicio || reserva.FechaInicio;
        const fechaFin = reserva.fechaFin || reserva.FechaFin;
        const numeroNoches = reserva.numeroNoches || reserva.NumeroNoches;
        const precioTotal = reserva.precioTotal || reserva.PrecioTotal;
        const estado = reserva.estado || reserva.Estado;
        const estadoPago = reserva.estadoPago || reserva.EstadoPago;

        const estadoColors = {
            'pendiente': '#ffc107',
            'confirmada': '#17a2b8',
            'completada': '#28a745',
            'cancelada': '#dc3545'
        };

        const pagoColors = {
            'pendiente': '#ffc107',
            'pagado': '#28a745',
            'reembolsado': '#6c757d'
        };

        return `
            <tr>
                <td><strong>${numeroReserva}</strong></td>
                <td>${usuario?.nombre || usuario?.Nombre || 'N/A'} ${usuario?.apellido || usuario?.Apellido || ''}</td>
                <td>${hotel?.nombre || hotel?.Nombre || 'N/A'}</td>
                <td>${habitacion?.numeroHabitacion || habitacion?.NumeroHabitacion || 'N/A'}</td>
                <td>${new Date(fechaInicio).toLocaleDateString('es-ES')}</td>
                <td>${new Date(fechaFin).toLocaleDateString('es-ES')}</td>
                <td>${numeroNoches}</td>
                <td><strong>S/ ${precioTotal?.toFixed(2)}</strong></td>
                <td>
                    <span style="
                        background: ${estadoColors[estado] || '#6c757d'};
                        color: white;
                        padding: 4px 12px;
                        border-radius: 16px;
                        font-size: 0.8rem;
                        font-weight: 500;
                        text-transform: capitalize;
                    ">
                        ${estado}
                    </span>
                </td>
                <td>
                    <span style="
                        background: ${pagoColors[estadoPago] || '#6c757d'};
                        color: white;
                        padding: 4px 12px;
                        border-radius: 16px;
                        font-size: 0.8rem;
                        font-weight: 500;
                        text-transform: capitalize;
                    ">
                        ${estadoPago}
                    </span>
                </td>
                <td>
                    <button class="btn btn-edit" onclick="window.ReservasModule.verDetalle(${reservaId})" title="Ver Detalles">
                        <i class="fas fa-eye"></i> Ver
                    </button>
                    ${estado === 'pendiente' ? `
                        <button class="btn btn-success" onclick="window.ReservasModule.confirmar(${reservaId})" title="Confirmar Reserva">
                            <i class="fas fa-check"></i> Confirmar
                        </button>
                    ` : ''}
                    ${(estado === 'pendiente' || estado === 'confirmada') ? `
                        <button class="btn btn-delete" onclick="window.ReservasModule.cancelar(${reservaId}, '${numeroReserva.replace(/'/g, "\\'")}')" title="Cancelar Reserva">
                            <i class="fas fa-times"></i> Cancelar
                        </button>
                    ` : ''}
                    ${estado === 'confirmada' ? `
                        <button class="btn btn-secondary" onclick="window.ReservasModule.completar(${reservaId})" title="Marcar como Completada">
                            <i class="fas fa-flag-checkered"></i> Completar
                        </button>
                    ` : ''}
                </td>
            </tr>
        `;
    },

    async search() {
        const busqueda = document.getElementById('searchReservas')?.value || '';
        const estado = document.getElementById('filterEstado')?.value || '';
        const estadoPago = document.getElementById('filterEstadoPago')?.value || '';

        try {
            const params = new URLSearchParams();
            if (estado) params.append('estado', estado);
            if (estadoPago) params.append('estadoPago', estadoPago);

            const response = await fetch(`${this.apiBase}/reservasapi?${params}`);
            const responseData = await response.json();
            let reservas = responseData?.data || responseData?.Data || responseData || [];

            // Filtro adicional de búsqueda en el cliente
            if (busqueda) {
                const busquedaLower = busqueda.toLowerCase();
                reservas = reservas.filter(reserva => {
                    const numeroReserva = (reserva.numeroReserva || reserva.NumeroReserva || '').toLowerCase();
                    const usuario = reserva.usuario || reserva.Usuario;
                    const nombreUsuario = ((usuario?.nombre || usuario?.Nombre || '') + ' ' + (usuario?.apellido || usuario?.Apellido || '')).toLowerCase();
                    
                    return numeroReserva.includes(busquedaLower) || nombreUsuario.includes(busquedaLower);
                });
            }

            const tbody = document.querySelector('#reservasTable tbody');
            if (tbody) {
                tbody.innerHTML = reservas.map(reserva => this.renderReservaRow(reserva)).join('');
            }
        } catch (error) {
            console.error('Error al buscar reservas:', error);
            showAlert('Error al buscar reservas: ' + error.message, 'error');
        }
    },

    async verDetalle(reservaId) {
        try {
            const response = await fetch(`${this.apiBase}/reservasapi/${reservaId}`);
            const responseData = await response.json();
            const reserva = responseData?.data || responseData?.Data || responseData;

            const usuario = reserva.usuario || reserva.Usuario;
            const habitacion = reserva.habitacion || reserva.Habitacion;
            const hotel = habitacion?.hotel || habitacion?.Hotel;

            const modalHtml = `
                <div class="modal show" id="detalleReservaModal" style="display: flex;">
                    <div class="modal-content" style="max-width: 700px;">
                        <div class="modal-header">
                            <h2 class="modal-title">📋 Detalle de Reserva</h2>
                            <button class="close" onclick="document.getElementById('detalleReservaModal').remove()">&times;</button>
                        </div>
                        <div class="modal-body">
                            <h3 style="color: #3498db; text-align: center; margin-bottom: 20px;">
                                ${reserva.numeroReserva || reserva.NumeroReserva}
                            </h3>
                            
                            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-bottom: 20px;">
                                <div>
                                    <h4 style="color: #2c3e50; margin-bottom: 10px;">👤 Usuario</h4>
                                    <p><strong>Nombre:</strong> ${usuario?.nombre || usuario?.Nombre} ${usuario?.apellido || usuario?.Apellido}</p>
                                    <p><strong>Email:</strong> ${usuario?.email || usuario?.Email}</p>
                                    <p><strong>Teléfono:</strong> ${usuario?.telefono || usuario?.Telefono}</p>
                                </div>
                                
                                <div>
                                    <h4 style="color: #2c3e50; margin-bottom: 10px;">🏨 Alojamiento</h4>
                                    <p><strong>Hotel:</strong> ${hotel?.nombre || hotel?.Nombre}</p>
                                    <p><strong>Habitación:</strong> ${habitacion?.numeroHabitacion || habitacion?.NumeroHabitacion}</p>
                                    <p><strong>Tipo:</strong> ${habitacion?.tipo || habitacion?.Tipo}</p>
                                </div>
                            </div>
                            
                            <div style="display: grid; grid-template-columns: 1fr 1fr; gap: 20px; margin-bottom: 20px;">
                                <div>
                                    <h4 style="color: #2c3e50; margin-bottom: 10px;">📅 Fechas</h4>
                                    <p><strong>Check-in:</strong> ${new Date(reserva.fechaInicio || reserva.FechaInicio).toLocaleDateString('es-ES')}</p>
                                    <p><strong>Check-out:</strong> ${new Date(reserva.fechaFin || reserva.FechaFin).toLocaleDateString('es-ES')}</p>
                                    <p><strong>Noches:</strong> ${reserva.numeroNoches || reserva.NumeroNoches}</p>
                                    <p><strong>Huéspedes:</strong> ${reserva.numeroHuespedes || reserva.NumeroHuespedes}</p>
                                </div>
                                
                                <div>
                                    <h4 style="color: #2c3e50; margin-bottom: 10px;">💰 Pago</h4>
                                    <p><strong>Precio Total:</strong> <span style="color: #3498db; font-size: 1.5rem;">S/ ${(reserva.precioTotal || reserva.PrecioTotal)?.toFixed(2)}</span></p>
                                    <p><strong>Estado:</strong> ${reserva.estado || reserva.Estado}</p>
                                    <p><strong>Estado Pago:</strong> ${reserva.estadoPago || reserva.EstadoPago}</p>
                                </div>
                            </div>
                            
                            ${(reserva.solicitudesEspeciales || reserva.SolicitudesEspeciales) ? `
                                <div style="background: #f8f9fa; padding: 15px; border-radius: 8px; margin-top: 20px;">
                                    <h4 style="color: #2c3e50; margin-bottom: 10px;">📝 Solicitudes Especiales</h4>
                                    <p>${reserva.solicitudesEspeciales || reserva.SolicitudesEspeciales}</p>
                                </div>
                            ` : ''}
                            
                            <div style="background: #e9ecef; padding: 15px; border-radius: 8px; margin-top: 20px;">
                                <p style="margin: 5px 0;"><strong>Creada:</strong> ${new Date(reserva.fechaCreacion || reserva.FechaCreacion).toLocaleString('es-ES')}</p>
                                ${(reserva.fechaModificacion || reserva.FechaModificacion) ? `
                                    <p style="margin: 5px 0;"><strong>Modificada:</strong> ${new Date(reserva.fechaModificacion || reserva.FechaModificacion).toLocaleString('es-ES')}</p>
                                ` : ''}
                            </div>
                        </div>
                        <div class="modal-footer">
                            <button type="button" class="btn btn-secondary" onclick="document.getElementById('detalleReservaModal').remove()">
                                Cerrar
                            </button>
                        </div>
                    </div>
                </div>
            `;

            document.body.insertAdjacentHTML('beforeend', modalHtml);

        } catch (error) {
            showAlert('Error al cargar los detalles de la reserva: ' + error.message, 'error');
        }
    },

    async confirmar(reservaId) {
        if (!confirm('¿Confirmar esta reserva? Esto marcará el pago como recibido.')) {
            return;
        }

        try {
            const response = await fetch(`${this.apiBase}/reservasapi/${reservaId}/confirmar`, {
                method: 'POST'
            });

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message || 'Reserva confirmada exitosamente', 'success');
                await window.adminApp.loadView('reservas');
            } else {
                throw new Error(result.message || result.Message || 'Error al confirmar la reserva');
            }
        } catch (error) {
            showAlert('Error: ' + error.message, 'error');
        }
    },

    async cancelar(reservaId, numeroReserva) {
        const motivo = prompt(`¿Estás seguro de cancelar la reserva ${numeroReserva}?\n\nIndica el motivo de la cancelación:`);
        
        if (!motivo || motivo.trim() === '') {
            return;
        }

        try {
            const response = await fetch(`${this.apiBase}/reservasapi/${reservaId}/cancelar`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify(motivo)
            });

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message || 'Reserva cancelada exitosamente', 'success');
                await window.adminApp.loadView('reservas');
            } else {
                throw new Error(result.message || result.Message || 'Error al cancelar la reserva');
            }
        } catch (error) {
            showAlert('Error: ' + error.message, 'error');
        }
    },

    async completar(reservaId) {
        if (!confirm('¿Marcar esta reserva como completada? Esta acción indica que el huésped ya ha hecho check-out.')) {
            return;
        }

        try {
            const response = await fetch(`${this.apiBase}/reservasapi/${reservaId}/completar`, {
                method: 'POST'
            });

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message || 'Reserva completada exitosamente', 'success');
                await window.adminApp.loadView('reservas');
            } else {
                throw new Error(result.message || result.Message || 'Error al completar la reserva');
            }
        } catch (error) {
            showAlert('Error: ' + error.message, 'error');
        }
    },

    async exportarReporte() {
        showAlert('Función de exportar reporte en desarrollo', 'success');
        // Aquí podrías implementar la exportación a CSV/Excel
    }
};