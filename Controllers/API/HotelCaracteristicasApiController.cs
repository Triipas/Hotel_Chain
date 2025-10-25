using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HotelCaracteristicasApiController : ControllerBase
    {
        private readonly IHotelCaracteristicaService _hotelCaracteristicaService;

        public HotelCaracteristicasApiController(IHotelCaracteristicaService hotelCaracteristicaService)
        {
            _hotelCaracteristicaService = hotelCaracteristicaService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var caracteristicas = await _hotelCaracteristicaService.GetAllAsync();
            return Ok(new { success = true, data = caracteristicas });
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var caracteristica = await _hotelCaracteristicaService.GetByIdAsync(id);
            if (caracteristica == null)
                return NotFound(new { success = false, message = "Característica no encontrada" });

            return Ok(new { success = true, data = caracteristica });
        }

        [HttpGet("hotel/{hotelId}")]
        public async Task<IActionResult> GetByHotelId(int hotelId)
        {
            var caracteristicas = await _hotelCaracteristicaService.GetByHotelIdAsync(hotelId);

            if (caracteristicas == null || !caracteristicas.Any())
                return NotFound(new { success = false, message = "No hay características registradas para este hotel" });

            return Ok(new { success = true, data = caracteristicas });
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HotelCaracteristica caracteristica)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var nuevaCaracteristica = await _hotelCaracteristicaService.CreateAsync(caracteristica);
            return Ok(new { success = true, message = "Característica agregada correctamente", data = nuevaCaracteristica });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HotelCaracteristica caracteristica)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var result = await _hotelCaracteristicaService.UpdateAsync(id, caracteristica);
            if (!result)
                return NotFound(new { success = false, message = "Característica no encontrada" });

            return Ok(new { success = true, message = "Característica actualizada correctamente" });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _hotelCaracteristicaService.DeleteAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "Característica no encontrada" });

            return Ok(new { success = true, message = "Característica eliminada correctamente" });
        }
    }
}