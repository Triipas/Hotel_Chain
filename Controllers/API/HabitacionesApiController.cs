using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models;
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
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetHabitaciones(
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

                return Ok(habitaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/habitaciones/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Habitacion>> GetHabitacion(int id)
        {
            try
            {
                var habitacion = await _habitacionService.GetByIdAsync(id);

                if (habitacion == null)
                {
                    return NotFound(new { message = $"Habitación con ID {id} no encontrada" });
                }

                return Ok(habitacion);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/habitaciones/hotel/5
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<IEnumerable<Habitacion>>> GetHabitacionesByHotel(int hotelId)
        {
            try
            {
                var habitaciones = await _habitacionService.GetByHotelIdAsync(hotelId);
                return Ok(habitaciones);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // POST: api/habitaciones
        [HttpPost]
        public async Task<ActionResult<Habitacion>> CreateHabitacion([FromBody] HabitacionCreateDto habitacionDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
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
                return Ok(new { 
                    success = true, 
                    message = "Habitación creada exitosamente", 
                    habitacion = createdHabitacion 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al crear la habitación", 
                    error = ex.Message 
                });
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
                    return BadRequest(ModelState);
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
                    return NotFound(new { 
                        success = false, 
                        message = $"Habitación con ID {id} no encontrada" 
                    });
                }

                return Ok(new { 
                    success = true, 
                    message = "Habitación actualizada exitosamente", 
                    habitacion = updatedHabitacion 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al actualizar la habitación", 
                    error = ex.Message 
                });
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
                    return NotFound(new { 
                        success = false, 
                        message = $"Habitación con ID {id} no encontrada" 
                    });
                }

                return Ok(new { 
                    success = true, 
                    message = "Habitación eliminada exitosamente" 
                });
            }
            catch (InvalidOperationException ex)
            {
                // Error de validación de negocio (ej. habitación con reservas)
                return BadRequest(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al eliminar la habitación", 
                    error = ex.Message 
                });
            }
        }
    }

    // DTOs para las operaciones
    public class HabitacionCreateDto
    {
        public int HotelId { get; set; }
        public string NumeroHabitacion { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public int Capacidad { get; set; }
        public decimal PrecioNoche { get; set; }
        public string? Descripcion { get; set; }
        public bool Disponible { get; set; } = true;
    }

    public class HabitacionUpdateDto
    {
        public int HotelId { get; set; }
        public string NumeroHabitacion { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public int Capacidad { get; set; }
        public decimal PrecioNoche { get; set; }
        public string? Descripcion { get; set; }
        public bool Disponible { get; set; } = true;
    }
}