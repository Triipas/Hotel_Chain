// Controllers/API/ReservasApiController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Reserva;
using Hotel_chain.Models.DTOs.Common;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasApiController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasApiController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // GET: api/reservas
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Reserva>>>> GetReservas(
            [FromQuery] string? estado,
            [FromQuery] string? estadoPago,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin)
        {
            try
            {
                IEnumerable<Reserva> reservas;

                if (!string.IsNullOrEmpty(estado) || !string.IsNullOrEmpty(estadoPago) || 
                    fechaInicio.HasValue || fechaFin.HasValue)
                {
                    reservas = await _reservaService.SearchAsync(estado, estadoPago, fechaInicio, fechaFin);
                }
                else
                {
                    reservas = await _reservaService.GetAllAsync();
                }

                return Ok(ApiResponse<IEnumerable<Reserva>>.SuccessResult(reservas, "Reservas obtenidas exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Reserva>>.ErrorResult(
                    "Error interno del servidor",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/reservas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Reserva>>> GetReserva(int id)
        {
            try
            {
                var reserva = await _reservaService.GetByIdAsync(id);

                if (reserva == null)
                {
                    return NotFound(ApiResponse<Reserva>.ErrorResult($"Reserva con ID {id} no encontrada"));
                }

                return Ok(ApiResponse<Reserva>.SuccessResult(reserva, "Reserva obtenida exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Reserva>.ErrorResult(
                    "Error interno del servidor",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/reservas/numero/BK-20240101-1234
        [HttpGet("numero/{numeroReserva}")]
        public async Task<ActionResult<ApiResponse<Reserva>>> GetReservaByNumero(string numeroReserva)
        {
            try
            {
                var reserva = await _reservaService.GetByNumeroReservaAsync(numeroReserva);

                if (reserva == null)
                {
                    return NotFound(ApiResponse<Reserva>.ErrorResult($"Reserva {numeroReserva} no encontrada"));
                }

                return Ok(ApiResponse<Reserva>.SuccessResult(reserva, "Reserva obtenida exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Reserva>.ErrorResult(
                    "Error interno del servidor",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/reservas/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Reserva>>>> GetReservasByUsuario(int usuarioId)
        {
            try
            {
                var reservas = await _reservaService.GetByUsuarioIdAsync(usuarioId);
                return Ok(ApiResponse<IEnumerable<Reserva>>.SuccessResult(
                    reservas,
                    "Reservas del usuario obtenidas exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Reserva>>.ErrorResult(
                    "Error interno del servidor",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/reservas/disponibilidad
        [HttpPost("disponibilidad")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Habitacion>>>> CheckDisponibilidad([FromBody] DisponibilidadSearchDto searchDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<IEnumerable<Habitacion>>.ErrorResult("Datos de entrada inválidos", errors));
                }

                var habitacionesDisponibles = await _reservaService.GetHabitacionesDisponiblesAsync(
                    searchDto.HotelId,
                    searchDto.FechaInicio,
                    searchDto.FechaFin,
                    searchDto.NumeroHuespedes
                );

                return Ok(ApiResponse<IEnumerable<Habitacion>>.SuccessResult(
                    habitacionesDisponibles,
                    $"Se encontraron {habitacionesDisponibles.Count()} habitaciones disponibles"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Habitacion>>.ErrorResult(
                    "Error al verificar disponibilidad",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/reservas
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Reserva>>> CreateReserva([FromBody] ReservaCreateDto reservaDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();

                    return BadRequest(ApiResponse<Reserva>.ErrorResult("Datos de entrada inválidos", errors));
                }

                var reserva = await _reservaService.CreateAsync(reservaDto);
                return Ok(ApiResponse<Reserva>.SuccessResult(reserva, "Reserva creada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<Reserva>.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Reserva>.ErrorResult(
                    "Error al crear la reserva",
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/reservas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateReserva(int id, [FromBody] ReservaUpdateDto reservaDto)
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

                var reserva = await _reservaService.UpdateAsync(id, reservaDto);

                if (reserva == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Reserva con ID {id} no encontrada"));
                }

                return Ok(ApiResponse<Reserva>.SuccessResult(reserva, "Reserva actualizada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al actualizar la reserva",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/reservas/5/confirmar
        [HttpPost("{id}/confirmar")]
        public async Task<IActionResult> ConfirmarReserva(int id)
        {
            try
            {
                var result = await _reservaService.ConfirmarReservaAsync(id);

                if (!result)
                {
                    return NotFound(ApiResponse.ErrorResult($"Reserva con ID {id} no encontrada"));
                }

                return Ok(ApiResponse.SuccessResult("Reserva confirmada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al confirmar la reserva",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/reservas/5/cancelar
        [HttpPost("{id}/cancelar")]
        public async Task<IActionResult> CancelarReserva(int id, [FromBody] string motivo)
        {
            try
            {
                var result = await _reservaService.CancelarReservaAsync(id, motivo ?? "Sin motivo especificado");

                if (!result)
                {
                    return NotFound(ApiResponse.ErrorResult($"Reserva con ID {id} no encontrada"));
                }

                return Ok(ApiResponse.SuccessResult("Reserva cancelada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al cancelar la reserva",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/reservas/5/completar
        [HttpPost("{id}/completar")]
        public async Task<IActionResult> CompletarReserva(int id)
        {
            try
            {
                var result = await _reservaService.CompletarReservaAsync(id);

                if (!result)
                {
                    return NotFound(ApiResponse.ErrorResult($"Reserva con ID {id} no encontrada"));
                }

                return Ok(ApiResponse.SuccessResult("Reserva completada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al completar la reserva",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/reservas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReserva(int id)
        {
            try
            {
                var deleted = await _reservaService.DeleteAsync(id);

                if (!deleted)
                {
                    return NotFound(ApiResponse.ErrorResult($"Reserva con ID {id} no encontrada"));
                }

                return Ok(ApiResponse.SuccessResult("Reserva eliminada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al eliminar la reserva",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/reservas/estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<ApiResponse<object>>> GetEstadisticas()
        {
            try
            {
                var totalActivas = await _reservaService.GetTotalReservasActivasAsync();
                var ingresosTotales = await _reservaService.GetIngresosTotalesAsync();
                var ingresosMes = await _reservaService.GetIngresosTotalesAsync(
                    DateTime.Now.AddMonths(-1),
                    DateTime.Now
                );

                var estadisticas = new
                {
                    ReservasActivas = totalActivas,
                    IngresosTotales = ingresosTotales,
                    IngresosUltimoMes = ingresosMes
                };

                return Ok(ApiResponse<object>.SuccessResult(estadisticas, "Estadísticas obtenidas exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResult(
                    "Error al obtener estadísticas",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}