// Controllers/Api/HabitacionesApiController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Habitacion;
using Hotel_chain.Models.DTOs.Common;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class HabitacionesApiController : ControllerBase
    {
        private readonly IHabitacionService _habitacionService;

        public HabitacionesApiController(IHabitacionService habitacionService)
        {
            _habitacionService = habitacionService;
        }

        // GET: api/habitaciones
        [HttpGet]
        public async Task<ActionResult<ApiResponse<IEnumerable<Habitacion>>>> GetHabitaciones(
            [FromQuery] int? hotelId, 
            [FromQuery] string? tipo, 
            [FromQuery] int? capacidadMinima)
        {
            try
            {
                IEnumerable<Habitacion> habitaciones;
                
                if (hotelId.HasValue || !string.IsNullOrEmpty(tipo) || capacidadMinima.HasValue)
                {
                    habitaciones = await _habitacionService.SearchAsync(hotelId, tipo, capacidadMinima);
                }
                else
                {
                    habitaciones = await _habitacionService.GetAllAsync();
                }

                return Ok(ApiResponse<IEnumerable<Habitacion>>.SuccessResult(habitaciones, "Habitaciones obtenidas exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Habitacion>>.ErrorResult(
                    "Error interno del servidor", 
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/habitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ApiResponse<Habitacion>>> GetHabitacion(int id)
        {
            try
            {
                var habitacion = await _habitacionService.GetByIdAsync(id);

                if (habitacion == null)
                {
                    return NotFound(ApiResponse<Habitacion>.ErrorResult($"Habitación con ID {id} no encontrada"));
                }

                return Ok(ApiResponse<Habitacion>.SuccessResult(habitacion, "Habitación obtenida exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Habitacion>.ErrorResult(
                    "Error interno del servidor", 
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/habitaciones/hotel/5
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<ApiResponse<IEnumerable<Habitacion>>>> GetHabitacionesByHotel(int hotelId)
        {
            try
            {
                var habitaciones = await _habitacionService.GetByHotelIdAsync(hotelId);
                return Ok(ApiResponse<IEnumerable<Habitacion>>.SuccessResult(
                    habitaciones, 
                    "Habitaciones del hotel obtenidas exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<IEnumerable<Habitacion>>.ErrorResult(
                    "Error interno del servidor", 
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/habitaciones
        [HttpPost]
        public async Task<ActionResult<ApiResponse<Habitacion>>> CreateHabitacion([FromBody] HabitacionCreateDto habitacionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse<Habitacion>.ErrorResult("Datos de entrada inválidos", errors));
                }

                var habitacion = new Habitacion
                {
                    HotelId = habitacionDto.HotelId,
                    NumeroHabitacion = habitacionDto.NumeroHabitacion,
                    Tipo = habitacionDto.Tipo,
                    Capacidad = habitacionDto.Capacidad,
                    PrecioNoche = habitacionDto.PrecioNoche,
                    Descripcion = habitacionDto.Descripcion,
                    Disponible = habitacionDto.Disponible
                };

                var createdHabitacion = await _habitacionService.CreateAsync(habitacion);
                return Ok(ApiResponse<Habitacion>.SuccessResult(createdHabitacion, "Habitación creada exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Habitacion>.ErrorResult(
                    "Error al crear la habitación", 
                    new List<string> { ex.Message }
                ));
            }
        }

        // PUT: api/habitaciones/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHabitacion(int id, [FromBody] HabitacionUpdateDto habitacionDto)
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

                var habitacion = new Habitacion
                {
                    HabitacionId = id,
                    HotelId = habitacionDto.HotelId,
                    NumeroHabitacion = habitacionDto.NumeroHabitacion,
                    Tipo = habitacionDto.Tipo,
                    Capacidad = habitacionDto.Capacidad,
                    PrecioNoche = habitacionDto.PrecioNoche,
                    Descripcion = habitacionDto.Descripcion,
                    Disponible = habitacionDto.Disponible
                };

                var updatedHabitacion = await _habitacionService.UpdateAsync(id, habitacion);
                
                if (updatedHabitacion == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Habitación con ID {id} no encontrada"));
                }

                return Ok(ApiResponse<Habitacion>.SuccessResult(updatedHabitacion, "Habitación actualizada exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al actualizar la habitación", 
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/habitaciones/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHabitacion(int id)
        {
            try
            {
                var deleted = await _habitacionService.DeleteAsync(id);
                
                if (!deleted)
                {
                    return NotFound(ApiResponse.ErrorResult($"Habitación con ID {id} no encontrada"));
                }

                return Ok(ApiResponse.SuccessResult("Habitación eliminada exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                // Error de validación de negocio (ej. habitación con reservas)
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al eliminar la habitación", 
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}