using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelAmenidadesApiController : ControllerBase
    {
        private readonly IHotelAmenidadService _hotelAmenidadService;

        public HotelAmenidadesApiController(IHotelAmenidadService hotelAmenidadService)
        {
            _hotelAmenidadService = hotelAmenidadService;
        }

        // ✅ GET: api/hotelamenidadesapi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenidades = await _hotelAmenidadService.GetAllAsync();
            return Ok(new { success = true, data = amenidades });
        }

        // ✅ GET: api/hotelamenidadesapi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenidad = await _hotelAmenidadService.GetByIdAsync(id);
            if (amenidad == null)
                return NotFound(new { success = false, message = "Amenidad no encontrada" });

            return Ok(new { success = true, data = amenidad });
        }

        // ✅ GET: api/hotelamenidadesapi/hotel/{hotelId}
        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotelId(int hotelId)
        {
            var amenidades = await _hotelAmenidadService.GetByHotelIdAsync(hotelId);

            if (amenidades == null || !amenidades.Any())
                return NotFound(new { success = false, message = "No hay amenidades registradas para este hotel" });

            return Ok(new { success = true, data = amenidades });
        }

        // ✅ POST: api/hotelamenidadesapi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HotelAmenidad amenidad)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var nuevaAmenidad = await _hotelAmenidadService.CreateAsync(amenidad);
            return Ok(new { success = true, message = "Amenidad agregada correctamente", data = nuevaAmenidad });
        }

        // ✅ PUT: api/hotelamenidadesapi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HotelAmenidad amenidad)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var result = await _hotelAmenidadService.UpdateAsync(id, amenidad);
            if (!result)
                return NotFound(new { success = false, message = "Amenidad no encontrada" });

            return Ok(new { success = true, message = "Amenidad actualizada correctamente" });
        }

        // ✅ DELETE: api/hotelamenidadesapi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _hotelAmenidadService.DeleteAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "Amenidad no encontrada" });

            return Ok(new { success = true, message = "Amenidad eliminada correctamente" });
        }
    }
}