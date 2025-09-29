// ============================================
// wwwroot/admin/js/admin-hotels.js
// ============================================

window.HotelesModule = {
    apiBase: '/api',
    isEditMode: false,
    currentEditingId: null,

    async render() {
        try {
            console.log('Cargando hoteles...');
            const response = await fetch(`${this.apiBase}/hotelesapi`);
            
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }
            
            const responseData = await response.json();
            console.log('Respuesta de API:', responseData);
            
            // Manejar tanto ApiResponse como array directo, con Data en PascalCase
            const hoteles = responseData?.data || responseData?.Data || responseData || [];
            
            console.log('Hoteles procesados:', hoteles);
            
            if (!Array.isArray(hoteles)) {
                console.error('Hoteles no es un array:', hoteles);
                throw new Error('La respuesta no es un array válido');
            }
            
            return `
                <section class="welcome-panel">
                    <div class="page-info">
                        <h1 class="panel-title">Gestión de Hoteles</h1>
                        <div class="toolbar-inline">
                            <input type="search" placeholder="Buscar hoteles por nombre o ciudad..." aria-label="Buscar hoteles" />
                            <button class="btn btn-secondary">
                                <i class="fas fa-filter"></i> Filtrar por Ciudad
                            </button>
                            <button class="btn btn-success" onclick="window.HotelesModule.openCreateModal()">
                                <i class="fas fa-plus"></i> Agregar Hotel
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
                                    <th>Nombre</th>
                                    <th>Ciudad</th>
                                    <th>Dirección</th>
                                    <th>Teléfono</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${hoteles.map(hotel => `
                                    <tr>
                                        <td>${hotel.hotelId || hotel.HotelId}</td>
                                        <td><strong>${hotel.nombre || hotel.Nombre}</strong></td>
                                        <td>${hotel.ciudad || hotel.Ciudad}</td>
                                        <td>${hotel.direccion || hotel.Direccion}</td>
                                        <td>${hotel.telefonoContacto || hotel.TelefonoContacto || 'N/A'}</td>
                                        <td>
                                            <button class="btn btn-edit" onclick="window.HotelesModule.edit(${hotel.hotelId || hotel.HotelId})">
                                                <i class="fas fa-edit"></i> Editar
                                            </button>
                                            <button class="btn btn-delete" onclick="window.HotelesModule.deleteHotel(${hotel.hotelId || hotel.HotelId}, '${(hotel.nombre || hotel.Nombre).replace(/'/g, "\\'")}')">
                                                <i class="fas fa-trash"></i> Eliminar
                                            </button>
                                        </td>
                                    </tr>
                                `).join('')}
                            </tbody>
                        </table>
                        ${hoteles.length === 0 ? '<div style="text-align: center; padding: 3rem; color: #666;"><i class="fas fa-hotel" style="font-size: 3rem; margin-bottom: 1rem; display: block;"></i>No hay hoteles registrados</div>' : ''}
                    </div>
                </div>
            `;
        } catch (error) {
            console.error('Error al cargar hoteles:', error);
            return `
                <div class="card">
                    <div class="card-body">
                        <h1>Error al cargar hoteles</h1>
                        <p><strong>Error:</strong> ${error.message}</p>
                        <p>Verifica la consola del navegador para más detalles.</p>
                    </div>
                </div>
            `;
        }
    },

    openCreateModal() {
        this.isEditMode = false;
        this.currentEditingId = null;
        document.getElementById('hotelModalTitle').textContent = 'Crear Hotel';
        document.getElementById('saveHotelText').textContent = 'Crear Hotel';
        document.getElementById('hotelForm').reset();
        document.getElementById('hotelId').value = '';
        document.getElementById('hotelModal').classList.add('show');
    },

    closeModal() {
        document.getElementById('hotelModal').classList.remove('show');
        document.getElementById('hotelForm').reset();
        this.currentEditingId = null;
        this.isEditMode = false;
    },

    async edit(hotelId) {
        try {
            const response = await fetch(`${this.apiBase}/hotelesapi/${hotelId}`);
            if (!response.ok) {
                throw new Error('Hotel no encontrado');
            }
            
            const responseData = await response.json();
            const hotel = responseData?.data || responseData?.Data || responseData;
            
            this.isEditMode = true;
            this.currentEditingId = hotelId;
            document.getElementById('hotelModalTitle').textContent = 'Editar Hotel';
            document.getElementById('saveHotelText').textContent = 'Actualizar Hotel';
            
            document.getElementById('hotelId').value = hotel.hotelId || hotel.HotelId;
            document.getElementById('nombre').value = hotel.nombre || hotel.Nombre;
            document.getElementById('direccion').value = hotel.direccion || hotel.Direccion;
            document.getElementById('ciudad').value = hotel.ciudad || hotel.Ciudad;
            document.getElementById('descripcion').value = hotel.descripcion || hotel.Descripcion || '';
            document.getElementById('telefonoContacto').value = hotel.telefonoContacto || hotel.TelefonoContacto || '';
            
            document.getElementById('hotelModal').classList.add('show');
        } catch (error) {
            showAlert('Error al cargar los datos del hotel: ' + error.message, 'error');
        }
    },

    async save() {
        const form = document.getElementById('hotelForm');
        const saveBtn = document.getElementById('saveHotelBtn');
        const saveText = document.getElementById('saveHotelText');
        
        if (!form.checkValidity()) {
            form.reportValidity();
            return;
        }

        saveBtn.disabled = true;
        saveText.innerHTML = '<span class="spinner"></span>Guardando...';

        try {
            const formData = new FormData(form);
            const hotelData = {
                nombre: formData.get('nombre'),
                direccion: formData.get('direccion'),
                ciudad: formData.get('ciudad'),
                descripcion: formData.get('descripcion') || null,
                telefonoContacto: formData.get('telefonoContacto') || null
            };

            let response;
            if (this.isEditMode && this.currentEditingId) {
                response = await fetch(`${this.apiBase}/hotelesapi/${this.currentEditingId}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(hotelData)
                });
            } else {
                response = await fetch(`${this.apiBase}/hotelesapi`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(hotelData)
                });
            }

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message, 'success');
                this.closeModal();
                await window.adminApp.loadView('hoteles');
            } else {
                throw new Error(result.message || result.Message || 'Error al guardar el hotel');
            }
        } catch (error) {
            showAlert('Error al guardar el hotel: ' + error.message, 'error');
        } finally {
            saveBtn.disabled = false;
            saveText.textContent = this.isEditMode ? 'Actualizar Hotel' : 'Crear Hotel';
        }
    },

    async deleteHotel(hotelId, hotelName) {
        if (!confirm(`¿Estás seguro de que quieres eliminar el hotel "${hotelName}"?\n\nEsta acción no se puede deshacer.`)) {
            return;
        }

        try {
            const response = await fetch(`${this.apiBase}/hotelesapi/${hotelId}`, {
                method: 'DELETE'
            });

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message, 'success');
                await window.adminApp.loadView('hoteles');
            } else {
                throw new Error(result.message || result.Message || 'Error al eliminar el hotel');
            }
        } catch (error) {
            showAlert('Error al eliminar el hotel: ' + error.message, 'error');
        }
    }
};