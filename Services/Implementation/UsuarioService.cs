// Services/Implementation/UsuarioService.cs
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class UsuarioService : IUsuarioService
    {
        private readonly AppDbContext _context;

        public UsuarioService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Usuario>> GetAllAsync()
        {
            return await _context.Usuarios
                .Include(u => u.Huesped)
                .Include(u => u.Staff)
                .Include(u => u.Reservas)
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Usuario?> GetByIdAsync(int id)
        {
            return await _context.Usuarios
                .Include(u => u.Huesped)
                .Include(u => u.Staff)
                .Include(u => u.Reservas)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);
        }

        public async Task<IEnumerable<Usuario>> SearchAsync(string? rol, string? estado, string? busqueda)
        {
            var query = _context.Usuarios
                .Include(u => u.Huesped)
                .Include(u => u.Staff)
                .Include(u => u.Reservas)
                .AsQueryable();

            if (!string.IsNullOrEmpty(rol))
                query = query.Where(u => u.Rol == rol);

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(u => u.Estado == estado);

            if (!string.IsNullOrEmpty(busqueda))
            {
                busqueda = busqueda.ToLower();
                query = query.Where(u => 
                    u.Nombre.ToLower().Contains(busqueda) ||
                    u.Apellido.ToLower().Contains(busqueda) ||
                    u.Email.ToLower().Contains(busqueda) ||
                    u.Documento.ToLower().Contains(busqueda)
                );
            }

            return await query
                .OrderByDescending(u => u.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Usuario> CreateAsync(Usuario usuario, string? rolDetallado = null, 
            string? permisosExtra = null, string? preferencias = null, string? notasInternas = null)
        {
            // Verificar que el email no exista
            if (await EmailExistsAsync(usuario.Email))
            {
                throw new InvalidOperationException($"El email '{usuario.Email}' ya está registrado");
            }

            usuario.FechaCreacion = DateTime.UtcNow;
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            // Crear registro en Huesped o Staff según el rol
            if (usuario.Rol == "huesped")
            {
                var huesped = new Huesped
                {
                    UsuarioId = usuario.UsuarioId,
                    Preferencias = preferencias,
                    NotasInternas = notasInternas
                };
                _context.Huespedes.Add(huesped);
            }
            else
            {
                var staff = new Staff
                {
                    UsuarioId = usuario.UsuarioId,
                    RolDetallado = rolDetallado ?? usuario.Rol,
                    PermisosExtra = permisosExtra
                };
                _context.Staff.Add(staff);
            }

            await _context.SaveChangesAsync();

            // Recargar con las relaciones
            return await GetByIdAsync(usuario.UsuarioId) ?? usuario;
        }

        public async Task<Usuario?> UpdateAsync(int id, Usuario usuario, string? rolDetallado = null, 
            string? permisosExtra = null, string? preferencias = null, string? notasInternas = null)
        {
            var existingUsuario = await _context.Usuarios
                .Include(u => u.Huesped)
                .Include(u => u.Staff)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            if (existingUsuario == null)
                return null;

            // Verificar que el email no esté en uso por otro usuario
            if (await EmailExistsAsync(usuario.Email, id))
            {
                throw new InvalidOperationException($"El email '{usuario.Email}' ya está registrado por otro usuario");
            }

            // Actualizar datos básicos
            existingUsuario.Nombre = usuario.Nombre;
            existingUsuario.Apellido = usuario.Apellido;
            existingUsuario.Email = usuario.Email;
            existingUsuario.Telefono = usuario.Telefono;
            existingUsuario.Documento = usuario.Documento;
            existingUsuario.Estado = usuario.Estado;

            // Actualizar contraseña solo si se proporciona una nueva
            if (!string.IsNullOrEmpty(usuario.Contraseña))
            {
                existingUsuario.Contraseña = usuario.Contraseña;
            }

            // Si el rol cambió, manejar el cambio
            if (existingUsuario.Rol != usuario.Rol)
            {
                // Eliminar registro anterior (Huesped o Staff)
                if (existingUsuario.Rol == "huesped" && existingUsuario.Huesped != null)
                {
                    _context.Huespedes.Remove(existingUsuario.Huesped);
                }
                else if (existingUsuario.Staff != null)
                {
                    _context.Staff.Remove(existingUsuario.Staff);
                }

                existingUsuario.Rol = usuario.Rol;

                // Crear nuevo registro
                if (usuario.Rol == "huesped")
                {
                    var huesped = new Huesped
                    {
                        UsuarioId = id,
                        Preferencias = preferencias,
                        NotasInternas = notasInternas
                    };
                    _context.Huespedes.Add(huesped);
                }
                else
                {
                    var staff = new Staff
                    {
                        UsuarioId = id,
                        RolDetallado = rolDetallado ?? usuario.Rol,
                        PermisosExtra = permisosExtra
                    };
                    _context.Staff.Add(staff);
                }
            }
            else
            {
                // Solo actualizar el registro existente
                if (usuario.Rol == "huesped" && existingUsuario.Huesped != null)
                {
                    existingUsuario.Huesped.Preferencias = preferencias;
                    existingUsuario.Huesped.NotasInternas = notasInternas;
                }
                else if (existingUsuario.Staff != null)
                {
                    existingUsuario.Staff.RolDetallado = rolDetallado ?? usuario.Rol;
                    existingUsuario.Staff.PermisosExtra = permisosExtra;
                }
            }

            await _context.SaveChangesAsync();

            // Recargar con las relaciones
            return await GetByIdAsync(id);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Reservas)
                .Include(u => u.Huesped)
                .Include(u => u.Staff)
                .FirstOrDefaultAsync(u => u.UsuarioId == id);

            if (usuario == null)
                return false;

            // Verificar si tiene reservas asociadas
            if (usuario.Reservas.Any())
            {
                throw new InvalidOperationException($"No se puede eliminar el usuario '{usuario.Nombre} {usuario.Apellido}' porque tiene reservas asociadas. Puedes desactivarlo cambiando su estado a 'inactivo'.");
            }

            // Eliminar registros relacionados
            if (usuario.Huesped != null)
            {
                _context.Huespedes.Remove(usuario.Huesped);
            }
            if (usuario.Staff != null)
            {
                _context.Staff.Remove(usuario.Staff);
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeUserId = null)
        {
            var query = _context.Usuarios.Where(u => u.Email.ToLower() == email.ToLower());

            if (excludeUserId.HasValue)
            {
                query = query.Where(u => u.UsuarioId != excludeUserId.Value);
            }

            return await query.AnyAsync();
        }
    }
}