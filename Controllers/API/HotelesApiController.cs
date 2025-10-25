// Controllers/Api/HotelesApiController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Hotel;
using Hotel_chain.Models.DTOs.Common;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.Api
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
public async Task<ActionResult<ApiResponse<IEnumerable<HotelResponseDto>>>> GetHoteles(
    [FromQuery] string? ubicacion, 
    [FromQuery] string? nombre)
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

        var hotelesDto = hoteles.Select(h => new HotelResponseDto
        {
            HotelId = h.HotelId,
            Nombre = h.Nombre,
            Direccion = h.Direccion,
            Ciudad = h.Ciudad,
            Pais = h.Pais,
            Estado = h.Estado,
            Latitud = h.Latitud,
            Longitud = h.Longitud,
            Descripcion = h.Descripcion,
            TelefonoContacto = h.TelefonoContacto,
            MascotasPermitidas = h.MascotasPermitidas,
            FumarPermitido = h.FumarPermitido,
            Moneda = h.Moneda,
            CheckInTime = h.CheckInTime,
            CheckOutTime = h.CheckOutTime,
            ContactoEmail = h.ContactoEmail,
            Calificacion = h.Calificacion,
            TotalHabitaciones = h.Habitaciones.Count,
            Imagenes = h.Imagenes.Select(i => i.NombreArchivo).ToList()
        }).ToList();

        return Ok(ApiResponse<IEnumerable<HotelResponseDto>>.SuccessResult(hotelesDto, "Hoteles obtenidos exitosamente"));
    }
    catch (Exception ex)
    {
        return StatusCode(500, ApiResponse<IEnumerable<HotelResponseDto>>.ErrorResult(
            "Error interno del servidor", 
            new List<string> { ex.Message }
        ));
    }
}

        [HttpGet("{id}")]
public async Task<ActionResult<ApiResponse<HotelResponseDto>>> GetHotel(int id)
{
    try
    {
        var hotel = await _hotelService.GetByIdAsync(id);

        if (hotel == null)
            return NotFound(ApiResponse<HotelResponseDto>.ErrorResult($"Hotel con ID {id} no encontrado"));

        var hotelDto = new HotelResponseDto
        {
            HotelId = hotel.HotelId,
            Nombre = hotel.Nombre,
            Direccion = hotel.Direccion,
            Ciudad = hotel.Ciudad,
            Pais = hotel.Pais,
            Estado = hotel.Estado,
            Latitud = hotel.Latitud,
            Longitud = hotel.Longitud,
            Descripcion = hotel.Descripcion,
            TelefonoContacto = hotel.TelefonoContacto,
            MascotasPermitidas = hotel.MascotasPermitidas,
            FumarPermitido = hotel.FumarPermitido,
            Moneda = hotel.Moneda,
            CheckInTime = hotel.CheckInTime,
            CheckOutTime = hotel.CheckOutTime,
            ContactoEmail = hotel.ContactoEmail,
            Calificacion = hotel.Calificacion,
            TotalHabitaciones = hotel.Habitaciones.Count,
            PoliticaCancelacion = hotel.PoliticaCancelacion,
            Imagenes = hotel.Imagenes.Select(i => i.NombreArchivo).ToList()
        };

        return Ok(ApiResponse<HotelResponseDto>.SuccessResult(hotelDto, "Hotel obtenido exitosamente"));
    }
    catch (Exception ex)
    {
        return StatusCode(500, ApiResponse<HotelResponseDto>.ErrorResult(
            "Error interno del servidor", 
            new List<string> { ex.Message }
        ));
    }
}

        [HttpPost]
        public async Task<ActionResult<ApiResponse<Hotel>>> CreateHotel([FromBody] HotelCreateDto hotelDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse<Hotel>.ErrorResult("Datos de entrada inválidos", errors));
                }

                var hotel = new Hotel
                {
                    Nombre = hotelDto.Nombre,
                    Direccion = hotelDto.Direccion,
                    Ciudad = hotelDto.Ciudad,
                    Descripcion = hotelDto.Descripcion,
                    TelefonoContacto = hotelDto.TelefonoContacto,
                    Pais = hotelDto.Pais,
    Estado = hotelDto.Estado,
    Latitud = hotelDto.Latitud,
    Longitud = hotelDto.Longitud,
    MascotasPermitidas = hotelDto.MascotasPermitidas,
    FumarPermitido = hotelDto.FumarPermitido,
    PoliticaCancelacion = hotelDto.PoliticaCancelacion
                };

                var createdHotel = await _hotelService.CreateAsync(hotel);
                return Ok(ApiResponse<Hotel>.SuccessResult(createdHotel, "Hotel creado exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<Hotel>.ErrorResult(
                    "Error al crear el hotel", 
                    new List<string> { ex.Message }
                ));
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
                    var errors = ModelState.Values
                        .SelectMany(v => v.Errors)
                        .Select(e => e.ErrorMessage)
                        .ToList();
                    
                    return BadRequest(ApiResponse.ErrorResult("Datos de entrada inválidos", errors));
                }

                var hotel = new Hotel
                {
                    HotelId = id,
                    Nombre = hotelDto.Nombre,
                    Direccion = hotelDto.Direccion,
                    Ciudad = hotelDto.Ciudad,
                    Descripcion = hotelDto.Descripcion,
                    TelefonoContacto = hotelDto.TelefonoContacto,
Pais = hotelDto.Pais,
    Estado = hotelDto.Estado,
    Latitud = hotelDto.Latitud,
    Longitud = hotelDto.Longitud,
    MascotasPermitidas = hotelDto.MascotasPermitidas,
    FumarPermitido = hotelDto.FumarPermitido,
    PoliticaCancelacion = hotelDto.PoliticaCancelacion
                };

                var updatedHotel = await _hotelService.UpdateAsync(id, hotel);
                
                if (updatedHotel == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Hotel con ID {id} no encontrado"));
                }

                return Ok(ApiResponse<Hotel>.SuccessResult(updatedHotel, "Hotel actualizado exitosamente"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al actualizar el hotel", 
                    new List<string> { ex.Message }
                ));
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
                    return NotFound(ApiResponse.ErrorResult($"Hotel con ID {id} no encontrado"));
                }

                return Ok(ApiResponse.SuccessResult("Hotel eliminado exitosamente"));
            }
            catch (InvalidOperationException ex)
            {
                // Error de validación de negocio (ej. hotel con habitaciones)
                return BadRequest(ApiResponse.ErrorResult(ex.Message));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al eliminar el hotel", 
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}