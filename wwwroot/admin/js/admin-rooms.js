// ============================================
// wwwroot/admin/js/admin-rooms.js
// ============================================

window.HabitacionesModule = {
    apiBase: '/api',
    isEditMode: false,
    currentEditingId: null,

    async render() {
        try {
            console.log('Cargando habitaciones...');
            const response = await fetch(`${this.apiBase}/habitacionesapi`);
            
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }
            
            const responseData = await response.json();
            console.log('Respuesta de API:', responseData);
            
            // Manejar tanto ApiResponse como array directo, con Data en PascalCase
            const habitaciones = responseData?.data || responseData?.Data || responseData || [];
            
            console.log('Habitaciones procesadas:', habitaciones);
            
            if (!Array.isArray(habitaciones)) {
                console.error('Habitaciones no es un array:', habitaciones);
                throw new Error('La respuesta no es un array válido');
            }
            
            return `
                <section class="welcome-panel">
                    <div class="page-info">
                        <h1 class="panel-title">Gestión de Habitaciones</h1>
                        <div class="toolbar-inline">
                            <input type="search" placeholder="Buscar habitaciones por número o hotel..." aria-label="Buscar habitaciones" />
                            <button class="btn btn-secondary">
                                <i class="fas fa-filter"></i> Filtrar por Tipo
                            </button>
                            <button class="btn btn-success" onclick="window.HabitacionesModule.openCreateModal()">
                                <i class="fas fa-plus"></i> Agregar Habitación
                            </button>
                        </div>
                    </div>
                </section>
                
                <div class="table-container">
                    <div class="table-wrapper">
                        <table>
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Hotel</th>
                                    <th>Número</th>
                                    <th>Tipo</th>
                                    <th>Capacidad</th>
                                    <th>Precio/Noche</th>
                                    <th>Disponible</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${habitaciones.map(hab => `
                                    <tr>
                                        <td>${hab.habitacionId || hab.HabitacionId}</td>
                                        <td><strong>${(hab.hotel && (hab.hotel.nombre || hab.hotel.Nombre)) || (hab.Hotel && (hab.Hotel.Nombre || hab.Hotel.nombre)) || 'N/A'}</strong></td>
                                        <td>${hab.numeroHabitacion || hab.NumeroHabitacion}</td>
                                        <td>
                                            <span style="
                                                background: ${(hab.tipo || hab.Tipo) === 'suite' ? '#f39c12' : 
                                                            (hab.tipo || hab.Tipo) === 'doble' ? '#3498db' : '#27ae60'};
                                                color: white;
                                                padding: 4px 12px;
                                                border-radius: 16px;
                                                font-size: 0.8rem;
                                                font-weight: 500;
                                                text-transform: capitalize;
                                            ">
                                                ${hab.tipo || hab.Tipo}
                                            </span>
                                        </td>
                                        <td>${hab.capacidad || hab.Capacidad} personas</td>
                                        <td><strong>S/ ${(hab.precioNoche || hab.PrecioNoche)?.toFixed(2)}</strong></td>
                                        <td>
                                            <span style="
                                                color: ${hab.disponible !== false && hab.Disponible !== false ? '#27ae60' : '#e74c3c'};
                                                font-weight: bold;
                                            ">
                                                ${hab.disponible !== false && hab.Disponible !== false ? '✓ Sí' : '✗ No'}
                                            </span>
                                        </td>
                                        <td>
                                         <button class="btn btn-info" onclick="window.HabitacionAmenidadesModule.verAmenidades(${hab.habitacionId || hab.HabitacionId})">
    <i class="fas fa-concierge-bell"></i> Amenidades
</button>
                                            <button class="btn btn-edit" onclick="window.HabitacionesModule.edit(${hab.habitacionId || hab.HabitacionId})">
                                                <i class="fas fa-edit"></i> Editar
                                            </button>
                                            <button class="btn btn-delete" onclick="window.HabitacionesModule.deleteHabitacion(${hab.habitacionId || hab.HabitacionId}, '${(hab.numeroHabitacion || hab.NumeroHabitacion).replace(/'/g, "\\'")}')">
                                                <i class="fas fa-trash"></i> Eliminar
                                            </button>
                                        </td>
                                    </tr>
                                `).join('')}
                            </tbody>
                        </table>
                        ${habitaciones.length === 0 ? '<div style="text-align: center; padding: 3rem; color: #666;"><i class="fas fa-bed" style="font-size: 3rem; margin-bottom: 1rem; display: block;"></i>No hay habitaciones registradas</div>' : ''}
                    </div>
                </div>
            `;
        } catch (error) {
            console.error('Error al cargar habitaciones:', error);
            return `
                <div class="card">
                    <div class="card-body">
                        <h1>Error al cargar habitaciones</h1>
                        <p><strong>Error:</strong> ${error.message}</p>
                        <p>Verifica la consola del navegador para más detalles.</p>
                    </div>
                </div>
            `;
        }
    },

    async openCreateModal() {
        this.isEditMode = false;
        this.currentEditingId = null;
        document.getElementById('habitacionModalTitle').textContent = 'Crear Habitación';
        document.getElementById('saveHabitacionText').textContent = 'Crear Habitación';
        document.getElementById('habitacionForm').reset();
        document.getElementById('habitacionId').value = '';
        document.getElementById('disponible').checked = true;
        
        document.getElementById('habitacionModal').classList.add('show');
        await this.loadHotelesDropdown();
        this.setupCapacidadAuto();
    },

    closeModal() {
        document.getElementById('habitacionModal').classList.remove('show');
        document.getElementById('habitacionForm').reset();
        this.currentEditingId = null;
        this.isEditMode = false;
    },
    
    setupCapacidadAuto() {
    const adultosInput = document.getElementById('capacidadAdultos');
    const ninosInput = document.getElementById('capacidadNinos');
    const capacidadTotalInput = document.getElementById('capacidad');

    function actualizarCapacidadTotal() {
        const adultos = parseInt(adultosInput.value) || 0;
        const ninos = parseInt(ninosInput.value) || 0;
        capacidadTotalInput.value = adultos + ninos;
    }

    adultosInput.addEventListener('input', actualizarCapacidadTotal);
    ninosInput.addEventListener('input', actualizarCapacidadTotal);

    // Actualizar inicialmente si ya hay valores
    actualizarCapacidadTotal();
},

    async loadHotelesDropdown() {
        try {
            const response = await fetch(`${this.apiBase}/hotelesapi`);
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }
            
            const responseData = await response.json();
            const hoteles = responseData?.data || responseData?.Data || responseData || [];
            
            const select = document.getElementById('habitacionHotelId');
            
            if (!select) {
                throw new Error('Elemento select no encontrado');
            }

            select.innerHTML = '<option value="">Seleccionar hotel</option>';
            
            hoteles.forEach(hotel => {
                const option = document.createElement('option');
                const hotelId = hotel.hotelId || hotel.HotelId;
                const hotelNombre = hotel.nombre || hotel.Nombre;
                
                option.value = hotelId;
                option.textContent = hotelNombre;
                select.appendChild(option);
            });

            return true;
        } catch (error) {
            console.error('Error al cargar hoteles:', error);
            showAlert('Error al cargar la lista de hoteles: ' + error.message, 'error');
            return false;
        }
    },

    async edit(habitacionId) {
        try {
            const response = await fetch(`${this.apiBase}/habitacionesapi/${habitacionId}`);
            if (!response.ok) {
                throw new Error('Habitación no encontrada');
            }
            
            const responseData = await response.json();
            const habitacion = responseData?.data || responseData?.Data || responseData;
            
            await this.loadHotelesDropdown();
            
            this.isEditMode = true;
            this.currentEditingId = habitacionId;
            document.getElementById('habitacionModalTitle').textContent = 'Editar Habitación';
            document.getElementById('saveHabitacionText').textContent = 'Actualizar Habitación';
            document.getElementById('nombreHabitacion').value = habitacion.nombre || habitacion.Nombre || '';
            document.getElementById('habitacionId').value = habitacion.habitacionId || habitacion.HabitacionId;
            document.getElementById('habitacionHotelId').value = habitacion.hotelId || habitacion.HotelId;
            document.getElementById('numeroHabitacion').value = habitacion.numeroHabitacion || habitacion.NumeroHabitacion;
            document.getElementById('tipo').value = habitacion.tipo || habitacion.Tipo;
            document.getElementById('capacidad').value = habitacion.capacidad || habitacion.Capacidad;
            document.getElementById('precioNoche').value = habitacion.precioNoche || habitacion.PrecioNoche;
            document.getElementById('descripcionHab').value = habitacion.descripcion || habitacion.Descripcion || '';
            document.getElementById('disponible').checked = habitacion.disponible !== false && habitacion.Disponible !== false;
            document.getElementById('capacidadAdultos').value = habitacion.capacidadAdultos || habitacion.CapacidadAdultos || '';
document.getElementById('capacidadNinos').value = habitacion.capacidadNinos || habitacion.CapacidadNinos || '';
document.getElementById('cantidadCamas').value = habitacion.cantidadCamas || habitacion.CantidadCamas || '';
document.getElementById('tipoCama').value = habitacion.tipoCama || habitacion.TipoCama || '';
document.getElementById('tamanoM2').value = habitacion.tamanoM2 || habitacion.TamanoM2 || '';
document.getElementById('precioBase').value = habitacion.precioBase || habitacion.PrecioBase || '';
document.getElementById('precioImpuestos').value = habitacion.precioImpuestos || habitacion.PrecioImpuestos || '';
document.getElementById('precioTotal').value = habitacion.precioTotal || habitacion.PrecioTotal || '';
document.getElementById('habitacionesDisponibles').value = habitacion.habitacionesDisponibles || habitacion.HabitacionesDisponibles || 0;
            
            
            document.getElementById('habitacionModal').classList.add('show');
        } catch (error) {
            showAlert('Error al cargar los datos de la habitación: ' + error.message, 'error');
        }
        this.setupCapacidadAuto();
    },

    async save() {
        const form = document.getElementById('habitacionForm');
        const saveBtn = document.getElementById('saveHabitacionBtn');
        const saveText = document.getElementById('saveHabitacionText');
        
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        saveBtn.disabled = true;
        saveText.innerHTML = '<span class="spinner"></span>Guardando...';

        try {
            const formData = new FormData(form);
            const habitacionData = {
                hotelId: parseInt(formData.get('hotelId')),
                numeroHabitacion: formData.get('numeroHabitacion'),
                 nombre: formData.get('nombre') || null,
                tipo: formData.get('tipo'),
                capacidad: parseInt(formData.get('capacidad')),
                precioNoche: parseFloat(formData.get('precioNoche')),
                descripcion: formData.get('descripcionHab') || null,
                disponible: document.getElementById('disponible').checked,
                capacidadAdultos: formData.get('capacidadAdultos') ? parseInt(formData.get('capacidadAdultos')) : null,
    capacidadNinos: formData.get('capacidadNinos') ? parseInt(formData.get('capacidadNinos')) : null,
    cantidadCamas: formData.get('cantidadCamas') ? parseInt(formData.get('cantidadCamas')) : null,
    tipoCama: formData.get('tipoCama') || null,
    tamanoM2: formData.get('tamanoM2') ? parseInt(formData.get('tamanoM2')) : null,
    precioBase: formData.get('precioBase') ? parseFloat(formData.get('precioBase')) : null,
    precioImpuestos: formData.get('precioImpuestos') ? parseFloat(formData.get('precioImpuestos')) : null,
    precioTotal: formData.get('precioTotal') ? parseFloat(formData.get('precioTotal')) : null,
    habitacionesDisponibles: formData.get('habitacionesDisponibles') ? parseInt(formData.get('habitacionesDisponibles')) : 0
            };

            let response;
            if (this.isEditMode && this.currentEditingId) {
                response = await fetch(`${this.apiBase}/habitacionesapi/${this.currentEditingId}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(habitacionData)
                });
            } else {
                response = await fetch(`${this.apiBase}/habitacionesapi`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(habitacionData)
                });
            }

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message, 'success');
                this.closeModal();
                await window.adminApp.loadView('habitaciones');
            } else {
                throw new Error(result.message || result.Message || 'Error al guardar la habitación');
            }
        } catch (error) {
            showAlert('Error al guardar la habitación: ' + error.message, 'error');
        } finally {
            saveBtn.disabled = false;
            saveText.textContent = this.isEditMode ? 'Actualizar Habitación' : 'Crear Habitación';
        }
    },

    async deleteHabitacion(habitacionId, numeroHabitacion) {
        if (!confirm(`¿Estás seguro de que quieres eliminar la habitación "${numeroHabitacion}"?\n\nEsta acción no se puede deshacer.`)) {
            return;
        }

        try {
            const response = await fetch(`${this.apiBase}/habitacionesapi/${habitacionId}`, {
                method: 'DELETE'
            });

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message, 'success');
                await window.adminApp.loadView('habitaciones');
            } else {
                throw new Error(result.message || result.Message || 'Error al eliminar la habitación');
            }
        } catch (error) {
            showAlert('Error al eliminar la habitación: ' + error.message, 'error');
        }
    }

    
};