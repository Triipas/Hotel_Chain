using Hotel_chain.Models.Entities;
using Hotel_chain.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Hotel_chain.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class HabitacionAmenidadesApiController : ControllerBase
    {
        private readonly IHabitacionAmenidadService _habitacionAmenidadService;

        public HabitacionAmenidadesApiController(IHabitacionAmenidadService habitacionAmenidadService)
        {
            _habitacionAmenidadService = habitacionAmenidadService;
        }

        // GET: api/habitacionamenidadesapi
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var amenidades = await _habitacionAmenidadService.GetAllAsync();
            return Ok(new { success = true, data = amenidades });
        }

        // GET: api/habitacionamenidadesapi/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var amenidad = await _habitacionAmenidadService.GetByIdAsync(id);
            if (amenidad == null)
                return NotFound(new { success = false, message = "Amenidad no encontrada" });

            return Ok(new { success = true, data = amenidad });
        }

        // GET: api/habitacionamenidadesapi/habitacion/{habitacionId}
        [HttpGet("habitacion/{habitacionId}")]
        public async Task<IActionResult> GetByHabitacionId(int habitacionId)
        {
            var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(habitacionId);

            if (amenidades == null || !amenidades.Any())
                return NotFound(new { success = false, message = "No hay amenidades registradas para esta habitación" });

            return Ok(new { success = true, data = amenidades });
        }

        // POST: api/habitacionamenidadesapi
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] HabitacionAmenidad amenidad)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var nuevaAmenidad = await _habitacionAmenidadService.CreateAsync(amenidad);
            return Ok(new { success = true, message = "Amenidad agregada correctamente", data = nuevaAmenidad });
        }

        // PUT: api/habitacionamenidadesapi/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] HabitacionAmenidad amenidad)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { success = false, message = "Datos inválidos" });

            var result = await _habitacionAmenidadService.UpdateAsync(id, amenidad);
            if (!result)
                return NotFound(new { success = false, message = "Amenidad no encontrada" });

            return Ok(new { success = true, message = "Amenidad actualizada correctamente" });
        }

        // DELETE: api/habitacionamenidadesapi/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _habitacionAmenidadService.DeleteAsync(id);
            if (!result)
                return NotFound(new { success = false, message = "Amenidad no encontrada" });

            return Ok(new { success = true, message = "Amenidad eliminada correctamente" });
        }
    }
}