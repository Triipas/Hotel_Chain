// Controllers/API/UsuariosApiController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Usuario;
using Hotel_chain.Models.DTOs.Common;
using Hotel_chain.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
            [FromQuery] string? estado,
            [FromQuery] string? busqueda)
        {
            try
            {
                IEnumerable<Usuario> usuarios;

                if (!string.IsNullOrEmpty(estado) || !string.IsNullOrEmpty(busqueda))
                    usuarios = await _usuarioService.SearchAsync(null, estado, busqueda);
                else
                    usuarios = await _usuarioService.GetAllAsync();

                return Ok(ApiResponse<IEnumerable<Usuario>>.SuccessResult(usuarios, "Usuarios obtenidos correctamente"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Usuario>>.ErrorResult(
                    "Error interno del servidor", new List<string> { ex.Message }));
            }
        }

        // GET: api/usuarios/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Usuario>>> GetUsuario(int id)
        {
            try
            {
                var usuario = await _usuarioService.GetByIdAsync(id);
                if (usuario == null)
                    return NotFound(ApiResponse<Usuario>.ErrorResult($"Usuario con ID {id} no encontrado"));

                return Ok(ApiResponse<Usuario>.SuccessResult(usuario, "Usuario obtenido correctamente"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<Usuario>.ErrorResult(
                    "Error interno del servidor", new List<string> { ex.Message }));
            }
        }

        // POST: api/usuarios
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Usuario>>> CreateUsuario([FromBody] UsuarioCreateDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Usuario>.ErrorResult("Datos inválidos", errors));
            }

            var usuario = new Usuario
            {
                Nombre = usuarioDto.Nombre,
                Apellido = usuarioDto.Apellido,
                Email = usuarioDto.Email,
                Telefono = usuarioDto.Telefono,
                Documento = usuarioDto.Documento,
                Contraseña = usuarioDto.Contraseña,
                Rol = "huesped", // fijo para huespedes
                Estado = usuarioDto.Estado ?? "activo"
            };

            try
            {
                var createdUsuario = await _usuarioService.CreateAsync(usuario);
                return Ok(ApiResponse<Usuario>.SuccessResult(createdUsuario, "Usuario creado correctamente"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<Usuario>.ErrorResult(
                    "Error al crear usuario", new List<string> { ex.Message }));
            }
        }

        // PUT: api/usuarios/{id}
        [HttpPut("{id}")]
        public async Task<ActionResult<ApiResponse<Usuario>>> UpdateUsuario(int id, [FromBody] UsuarioUpdateDto usuarioDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return BadRequest(ApiResponse<Usuario>.ErrorResult("Datos inválidos", errors));
            }

            var usuario = new Usuario
            {
                UsuarioId = id,
                Nombre = usuarioDto.Nombre,
                Apellido = usuarioDto.Apellido,
                Email = usuarioDto.Email,
                Telefono = usuarioDto.Telefono,
                Documento = usuarioDto.Documento,
                Contraseña = usuarioDto.Contraseña ?? "", // opcional
                Rol = "huesped",
                Estado = usuarioDto.Estado ?? "activo"
            };

            try
            {
                var updatedUsuario = await _usuarioService.UpdateAsync(id, usuario);
                if (updatedUsuario == null)
                    return NotFound(ApiResponse<Usuario>.ErrorResult($"Usuario con ID {id} no encontrado"));

                return Ok(ApiResponse<Usuario>.SuccessResult(updatedUsuario, "Usuario actualizado correctamente"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<Usuario>.ErrorResult(
                    "Error al actualizar usuario", new List<string> { ex.Message }));
            }
        }

        // DELETE: api/usuarios/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult<ApiResponse<string>>> DeleteUsuario(int id)
        {
            try
            {
                var deleted = await _usuarioService.DeleteAsync(id);
                if (!deleted)
                    return NotFound(ApiResponse<string>.ErrorResult($"Usuario con ID {id} no encontrado"));

                return Ok(ApiResponse<string>.SuccessResult("Usuario eliminado correctamente"));
            }
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<string>.ErrorResult(
                    "Error al eliminar usuario", new List<string> { ex.Message }));
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
            catch (System.Exception ex)
            {
                return StatusCode(500, ApiResponse<bool>.ErrorResult(
                    "Error al verificar email", new List<string> { ex.Message }));
            }
        }
    }
}