// Controllers/API/UsuariosApiController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Usuario;
using Hotel_chain.Models.DTOs.Common;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosApiController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;

        public UsuariosApiController(IUsuarioService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: api/usuarios
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Usuario>>>> GetUsuarios(
            [FromQuery] string? rol,
            [FromQuery] string? estado,
            [FromQuery] string? busqueda)
        {
            try
            {
                IEnumerable<Usuario> usuarios;

                if (!string.IsNullOrEmpty(rol) || !string.IsNullOrEmpty(estado) || !string.IsNullOrEmpty(busqueda))
                {
                    usuarios = await _usuarioService.SearchAsync(rol, estado, busqueda);
                }
                else
                {
                    usuarios = await _usuarioService.GetAllAsync();
                }

                return Ok(ApiResponse<IEnumerable<Usuario>>.SuccessResult(usuarios, "Usuarios obtenidos exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Usuario>>.ErrorResult(
                    "Error interno del servidor",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Usuario>>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);

                if (usuario == null)
                {
                    return NotFound(ApiResponse<Usuario>.ErrorResult($"Usuario con ID {id} no encontrado"));
                }

                return Ok(ApiResponse<Usuario>.SuccessResult(usuario, "Usuario obtenido exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Usuario>.ErrorResult(
                    "Error interno del servidor",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Usuario>>> CreateUsuario([FromBody] UsuarioCreateDto usuarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<Usuario>.ErrorResult("Datos de entrada inválidos", errors));
                }

                var usuario = new Usuario
                {
                    Nombre = usuarioDto.Nombre,
                    Apellido = usuarioDto.Apellido,
                    Email = usuarioDto.Email,
                    Telefono = usuarioDto.Telefono,
                    Documento = usuarioDto.Documento,
                    Contraseña = usuarioDto.Contraseña,
                    Rol = usuarioDto.Rol,
                    Estado = usuarioDto.Estado
                };

                var createdUsuario = await _usuarioService.CreateAsync(
                    usuario,
                    usuarioDto.RolDetallado,
                    usuarioDto.PermisosExtra,
                    usuarioDto.Preferencias,
                    usuarioDto.NotasInternas
                );

                return Ok(ApiResponse<Usuario>.SuccessResult(createdUsuario, "Usuario creado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<Usuario>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Usuario>.ErrorResult(
                    "Error al crear el usuario",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto usuarioDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse.ErrorResult("Datos de entrada inválidos", errors));
                }

                var usuario = new Usuario
                {
                    UsuarioId = id,
                    Nombre = usuarioDto.Nombre,
                    Apellido = usuarioDto.Apellido,
                    Email = usuarioDto.Email,
                    Telefono = usuarioDto.Telefono,
                    Documento = usuarioDto.Documento,
                    Contraseña = usuarioDto.Contraseña ?? "", // Se maneja en el servicio
                    Rol = usuarioDto.Rol,
                    Estado = usuarioDto.Estado
                };

                var updatedUsuario = await _usuarioService.UpdateAsync(
                    id,
                    usuario,
                    usuarioDto.RolDetallado,
                    usuarioDto.PermisosExtra,
                    usuarioDto.Preferencias,
                    usuarioDto.NotasInternas
                );

                if (updatedUsuario == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Usuario con ID {id} no encontrado"));
                }

                return Ok(ApiResponse<Usuario>.SuccessResult(updatedUsuario, "Usuario actualizado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al actualizar el usuario",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            try
            {
                var deleted = await _usuarioService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(ApiResponse.ErrorResult($"Usuario con ID {id} no encontrado"));
                }

                return Ok(ApiResponse.SuccessResult("Usuario eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al eliminar el usuario",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/usuarios/check-email?email=test@test.com&excludeId=5
        [HttpGet("check-email")]
        public async Task<ActionResult<ApiResponse<bool>>> CheckEmailExists(
            [FromQuery] string email,
            [FromQuery] int? excludeId = null)
        {
            try
            {
                var exists = await _usuarioService.EmailExistsAsync(email, excludeId);
                return Ok(ApiResponse<bool>.SuccessResult(exists, exists ? "Email ya está en uso" : "Email disponible"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult(
                    "Error al verificar email",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}