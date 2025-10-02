// wwwroot/admin/js/admin-users.js

window.UsuariosModule = {
    apiBase: '/api',
    isEditMode: false,
    currentEditingId: null,

    async render() {
        try {
            console.log('Cargando usuarios...');
            const response = await fetch(`${this.apiBase}/usuariosapi`);
            
            if (!response.ok) {
                throw new Error(`HTTP Error: ${response.status}`);
            }
            
            const responseData = await response.json();
            console.log('Respuesta de API:', responseData);
            
            const usuarios = responseData?.data || responseData?.Data || responseData || [];
            
            console.log('Usuarios procesados:', usuarios);
            
            if (!Array.isArray(usuarios)) {
                console.error('Usuarios no es un array:', usuarios);
                throw new Error('La respuesta no es un array válido');
            }
            
            return `
                <section class="welcome-panel">
                    <div class="page-info">
                        <h1 class="panel-title">Gestión de Usuarios</h1>
                        <div class="toolbar-inline">
                            <input type="search" id="searchUsuarios" placeholder="Buscar por nombre, email o documento..." 
                                   aria-label="Buscar usuarios" onkeyup="window.UsuariosModule.search()" />
                            <select id="filterRol" onchange="window.UsuariosModule.search()">
                                <option value="">Todos los roles</option>
                                <option value="huesped">Huésped</option>
                                <option value="admin">Admin</option>
                                <option value="recepcionista">Recepcionista</option>
                                <option value="gerente">Gerente</option>
                                <option value="dueño">Dueño</option>
                            </select>
                            <select id="filterEstado" onchange="window.UsuariosModule.search()">
                                <option value="">Todos los estados</option>
                                <option value="activo">Activo</option>
                                <option value="inactivo">Inactivo</option>
                            </select>
                            <button class="btn btn-success" onclick="window.UsuariosModule.openCreateModal()">
                                <i class="fas fa-user-plus"></i> Agregar Usuario
                            </button>
                        </div>
                    </div>
                </section>
                
                <div class="table-container">
                    <div class="table-wrapper">
                        <table id="usuariosTable">
                            <thead>
                                <tr>
                                    <th>ID</th>
                                    <th>Nombre Completo</th>
                                    <th>Email</th>
                                    <th>Documento</th>
                                    <th>Rol</th>
                                    <th>Estado</th>
                                    <th>Último Acceso</th>
                                    <th>Acciones</th>
                                </tr>
                            </thead>
                            <tbody>
                                ${usuarios.map(user => this.renderUsuarioRow(user)).join('')}
                            </tbody>
                        </table>
                        ${usuarios.length === 0 ? '<div style="text-align: center; padding: 3rem; color: #666;"><i class="fas fa-users" style="font-size: 3rem; margin-bottom: 1rem; display: block;"></i>No hay usuarios registrados</div>' : ''}
                    </div>
                </div>
            `;
        } catch (error) {
            console.error('Error al cargar usuarios:', error);
            return `
                <div class="card">
                    <div class="card-body">
                        <h1>Error al cargar usuarios</h1>
                        <p><strong>Error:</strong> ${error.message}</p>
                        <p>Verifica la consola del navegador para más detalles.</p>
                    </div>
                </div>
            `;
        }
    },

    renderUsuarioRow(user) {
        const usuarioId = user.usuarioId || user.UsuarioId;
        const nombre = user.nombre || user.Nombre;
        const apellido = user.apellido || user.Apellido;
        const email = user.email || user.Email;
        const documento = user.documento || user.Documento;
        const rol = user.rol || user.Rol;
        const estado = user.estado || user.Estado;
        const ultimoAcceso = user.ultimoAcceso || user.UltimoAcceso;

        const rolColor = {
            'huesped': '#3498db',
            'admin': '#e74c3c',
            'recepcionista': '#f39c12',
            'gerente': '#9b59b6',
            'dueño': '#2ecc71'
        };

        const estadoColor = estado === 'activo' ? '#27ae60' : '#95a5a6';

        return `
            <tr>
                <td>${usuarioId}</td>
                <td><strong>${nombre} ${apellido}</strong></td>
                <td>${email}</td>
                <td>${documento}</td>
                <td>
                    <span style="
                        background: ${rolColor[rol] || '#95a5a6'};
                        color: white;
                        padding: 4px 12px;
                        border-radius: 16px;
                        font-size: 0.8rem;
                        font-weight: 500;
                        text-transform: capitalize;
                    ">
                        ${rol}
                    </span>
                </td>
                <td>
                    <span style="color: ${estadoColor}; font-weight: bold;">
                        ${estado === 'activo' ? '✓ Activo' : '✗ Inactivo'}
                    </span>
                </td>
                <td>${ultimoAcceso ? new Date(ultimoAcceso).toLocaleDateString('es-ES') : 'Nunca'}</td>
                <td>
                    <button class="btn btn-edit" onclick="window.UsuariosModule.edit(${usuarioId})">
                        <i class="fas fa-edit"></i> Editar
                    </button>
                    <button class="btn btn-delete" onclick="window.UsuariosModule.deleteUsuario(${usuarioId}, '${(nombre + ' ' + apellido).replace(/'/g, "\\'")}')">
                        <i class="fas fa-trash"></i> Eliminar
                    </button>
                </td>
            </tr>
        `;
    },

    async search() {
        const busqueda = document.getElementById('searchUsuarios')?.value || '';
        const rol = document.getElementById('filterRol')?.value || '';
        const estado = document.getElementById('filterEstado')?.value || '';

        try {
            const params = new URLSearchParams();
            if (busqueda) params.append('busqueda', busqueda);
            if (rol) params.append('rol', rol);
            if (estado) params.append('estado', estado);

            const response = await fetch(`${this.apiBase}/usuariosapi?${params}`);
            const responseData = await response.json();
            const usuarios = responseData?.data || responseData?.Data || responseData || [];

            const tbody = document.querySelector('#usuariosTable tbody');
            if (tbody) {
                tbody.innerHTML = usuarios.map(user => this.renderUsuarioRow(user)).join('');
            }
        } catch (error) {
            console.error('Error al buscar usuarios:', error);
            showAlert('Error al buscar usuarios: ' + error.message, 'error');
        }
    },

    openCreateModal() {
        this.isEditMode = false;
        this.currentEditingId = null;
        document.getElementById('usuarioModalTitle').textContent = 'Crear Usuario';
        document.getElementById('saveUsuarioText').textContent = 'Crear Usuario';
        document.getElementById('usuarioForm').reset();
        document.getElementById('usuarioId').value = '';
        document.getElementById('passwordGroup').style.display = 'block';
        document.getElementById('passwordLabel').innerHTML = 'Contraseña *';
        document.getElementById('usuarioRol').value = 'huesped';
        this.toggleRolFields('huesped');
        document.getElementById('usuarioModal').classList.add('show');
    },

    closeModal() {
        document.getElementById('usuarioModal').classList.remove('show');
        document.getElementById('usuarioForm').reset();
        this.currentEditingId = null;
        this.isEditMode = false;
    },

    toggleRolFields(rol) {
        const staffFields = document.getElementById('staffFields');
        const huespedFields = document.getElementById('huespedFields');

        if (rol === 'huesped') {
            staffFields.style.display = 'none';
            huespedFields.style.display = 'block';
        } else {
            staffFields.style.display = 'block';
            huespedFields.style.display = 'none';
        }
    },

    async edit(usuarioId) {
        try {
            const response = await fetch(`${this.apiBase}/usuariosapi/${usuarioId}`);
            if (!response.ok) {
                throw new Error('Usuario no encontrado');
            }
            
            const responseData = await response.json();
            const usuario = responseData?.data || responseData?.Data || responseData;
            
            this.isEditMode = true;
            this.currentEditingId = usuarioId;
            document.getElementById('usuarioModalTitle').textContent = 'Editar Usuario';
            document.getElementById('saveUsuarioText').textContent = 'Actualizar Usuario';
            
            document.getElementById('usuarioId').value = usuario.usuarioId || usuario.UsuarioId;
            document.getElementById('usuarioNombre').value = usuario.nombre || usuario.Nombre;
            document.getElementById('usuarioApellido').value = usuario.apellido || usuario.Apellido;
            document.getElementById('usuarioEmail').value = usuario.email || usuario.Email;
            document.getElementById('usuarioTelefono').value = usuario.telefono || usuario.Telefono;
            document.getElementById('usuarioDocumento').value = usuario.documento || usuario.Documento;
            document.getElementById('usuarioRol').value = usuario.rol || usuario.Rol;
            document.getElementById('usuarioEstado').value = usuario.estado || usuario.Estado;
            
            // Contraseña opcional en edición
            document.getElementById('passwordGroup').style.display = 'block';
            document.getElementById('passwordLabel').innerHTML = 'Contraseña <small>(dejar vacío para no cambiar)</small>';
            document.getElementById('usuarioPassword').value = '';
            document.getElementById('usuarioPassword').removeAttribute('required');
            
            // Campos específicos según rol
            const rol = usuario.rol || usuario.Rol;
            this.toggleRolFields(rol);
            
            if (rol === 'huesped') {
                const huesped = usuario.huesped || usuario.Huesped;
                if (huesped) {
                    document.getElementById('huespedPreferencias').value = huesped.preferencias || huesped.Preferencias || '';
                    document.getElementById('huespedNotas').value = huesped.notasInternas || huesped.NotasInternas || '';
                }
            } else {
                const staff = usuario.staff || usuario.Staff;
                if (staff) {
                    document.getElementById('staffRolDetallado').value = staff.rolDetallado || staff.RolDetallado || '';
                    document.getElementById('staffPermisos').value = staff.permisosExtra || staff.PermisosExtra || '';
                }
            }
            
            document.getElementById('usuarioModal').classList.add('show');
        } catch (error) {
            showAlert('Error al cargar los datos del usuario: ' + error.message, 'error');
        }
    },

    async save() {
        const form = document.getElementById('usuarioForm');
        const saveBtn = document.getElementById('saveUsuarioBtn');
        const saveText = document.getElementById('saveUsuarioText');
        
        // Validar campos requeridos manualmente
        const nombre = document.getElementById('usuarioNombre').value;
        const apellido = document.getElementById('usuarioApellido').value;
        const email = document.getElementById('usuarioEmail').value;
        const telefono = document.getElementById('usuarioTelefono').value;
        const documento = document.getElementById('usuarioDocumento').value;
        const password = document.getElementById('usuarioPassword').value;
        const rol = document.getElementById('usuarioRol').value;

        if (!nombre || !apellido || !email || !telefono || !documento || !rol) {
            showAlert('Por favor completa todos los campos obligatorios', 'error');
            return;
        }

        if (!this.isEditMode && !password) {
            showAlert('La contraseña es obligatoria para nuevos usuarios', 'error');
            return;
        }

        saveBtn.disabled = true;
        saveText.innerHTML = '<span class="spinner"></span>Guardando...';

        try {
            const formData = new FormData(form);
            const usuarioData = {
                nombre: formData.get('nombre'),
                apellido: formData.get('apellido'),
                email: formData.get('email'),
                telefono: formData.get('telefono'),
                documento: formData.get('documento'),
                rol: formData.get('rol'),
                estado: formData.get('estado'),
                contraseña: formData.get('password') || null,
                rolDetallado: formData.get('rolDetallado') || null,
                permisosExtra: formData.get('permisosExtra') || null,
                preferencias: formData.get('preferencias') || null,
                notasInternas: formData.get('notasInternas') || null
            };

            let response;
            if (this.isEditMode && this.currentEditingId) {
                response = await fetch(`${this.apiBase}/usuariosapi/${this.currentEditingId}`, {
                    method: 'PUT',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(usuarioData)
                });
            } else {
                response = await fetch(`${this.apiBase}/usuariosapi`, {
                    method: 'POST',
                    headers: { 'Content-Type': 'application/json' },
                    body: JSON.stringify(usuarioData)
                });
            }

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message, 'success');
                this.closeModal();
                await window.adminApp.loadView('usuarios');
            } else {
                throw new Error(result.message || result.Message || 'Error al guardar el usuario');
            }
        } catch (error) {
            showAlert('Error al guardar el usuario: ' + error.message, 'error');
        } finally {
            saveBtn.disabled = false;
            saveText.textContent = this.isEditMode ? 'Actualizar Usuario' : 'Crear Usuario';
        }
    },

    async deleteUsuario(usuarioId, nombreCompleto) {
        if (!confirm(`¿Estás seguro de que quieres eliminar al usuario "${nombreCompleto}"?\n\nSi tiene reservas asociadas, no podrá ser eliminado y deberás desactivarlo en su lugar.\n\nEsta acción no se puede deshacer.`)) {
            return;
        }

        try {
            const response = await fetch(`${this.apiBase}/usuariosapi/${usuarioId}`, {
                method: 'DELETE'
            });

            const result = await response.json();

            if (response.ok && (result.success || result.Success)) {
                showAlert(result.message || result.Message, 'success');
                await window.adminApp.loadView('usuarios');
            } else {
                throw new Error(result.message || result.Message || 'Error al eliminar el usuario');
            }
        } catch (error) {
            showAlert('Error: ' + error.message, 'error');
        }
    }
};