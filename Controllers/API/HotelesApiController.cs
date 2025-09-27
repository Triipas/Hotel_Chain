using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelesApiController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelesApiController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        // GET: api/hoteles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Hotel>>> GetHoteles([FromQuery] string? ubicacion, [FromQuery] string? nombre)
        {
            try
            {
                IEnumerable<Hotel> hoteles;
                
                if (!string.IsNullOrEmpty(ubicacion) || !string.IsNullOrEmpty(nombre))
                {
                    hoteles = await _hotelService.SearchAsync(ubicacion, nombre);
                }
                else
                {
                    hoteles = await _hotelService.GetAllAsync();
                }

                return Ok(hoteles);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // GET: api/hoteles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            try
            {
                var hotel = await _hotelService.GetByIdAsync(id);

                if (hotel == null)
                {
                    return NotFound(new { message = $"Hotel con ID {id} no encontrado" });
                }

                return Ok(hotel);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor", error = ex.Message });
            }
        }

        // POST: api/hoteles
        [HttpPost]
        public async Task<ActionResult<Hotel>> CreateHotel([FromBody] HotelCreateDto hotelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var hotel = new Hotel
                {
                    Nombre = hotelDto.Nombre,
                    Direccion = hotelDto.Direccion,
                    Ciudad = hotelDto.Ciudad,
                    Descripcion = hotelDto.Descripcion,
                    TelefonoContacto = hotelDto.TelefonoContacto
                };

                var createdHotel = await _hotelService.CreateAsync(hotel);
                return Ok(new { 
                    success = true, 
                    message = "Hotel creado exitosamente", 
                    hotel = createdHotel 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al crear el hotel", 
                    error = ex.Message 
                });
            }
        }

        // PUT: api/hoteles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, [FromBody] HotelUpdateDto hotelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var hotel = new Hotel
                {
                    HotelId = id,
                    Nombre = hotelDto.Nombre,
                    Direccion = hotelDto.Direccion,
                    Ciudad = hotelDto.Ciudad,
                    Descripcion = hotelDto.Descripcion,
                    TelefonoContacto = hotelDto.TelefonoContacto
                };

                var updatedHotel = await _hotelService.UpdateAsync(id, hotel);
                
                if (updatedHotel == null)
                {
                    return NotFound(new { 
                        success = false, 
                        message = $"Hotel con ID {id} no encontrado" 
                    });
                }

                return Ok(new { 
                    success = true, 
                    message = "Hotel actualizado exitosamente", 
                    hotel = updatedHotel 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al actualizar el hotel", 
                    error = ex.Message 
                });
            }
        }

        // DELETE: api/hoteles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            try
            {
                var deleted = await _hotelService.DeleteAsync(id);
                
                if (!deleted)
                {
                    return NotFound(new { 
                        success = false, 
                        message = $"Hotel con ID {id} no encontrado" 
                    });
                }

                return Ok(new { 
                    success = true, 
                    message = "Hotel eliminado exitosamente" 
                });
            }
            catch (InvalidOperationException ex)
            {
                // Error de validación de negocio (ej. hotel con habitaciones)
                return BadRequest(new { 
                    success = false, 
                    message = ex.Message 
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    success = false, 
                    message = "Error al eliminar el hotel", 
                    error = ex.Message 
                });
            }
        }
    }

    // DTOs para las operaciones
    public class HotelCreateDto
    {
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? TelefonoContacto { get; set; }
    }

    public class HotelUpdateDto
    {
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string? Descripcion { get; set; }
        public string? TelefonoContacto { get; set; }
    }
}