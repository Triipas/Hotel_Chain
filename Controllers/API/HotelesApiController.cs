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

        // GET: api/hoteles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Hotel>> GetHotel(int id)
        {
            var hotel = await _hotelService.GetByIdAsync(id);

            if (hotel == null)
            {
                return NotFound(new { message = $"Hotel con ID {id} no encontrado" });
            }

            return Ok(hotel);
        }

        // POST: api/hoteles
        [HttpPost]
        public async Task<ActionResult<Hotel>> CreateHotel(Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdHotel = await _hotelService.CreateAsync(hotel);
            return CreatedAtAction(nameof(GetHotel), new { id = createdHotel.HotelId }, createdHotel);
        }

        // PUT: api/hoteles/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateHotel(int id, Hotel hotel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var updatedHotel = await _hotelService.UpdateAsync(id, hotel);
            
            if (updatedHotel == null)
            {
                return NotFound(new { message = $"Hotel con ID {id} no encontrado" });
            }

            return Ok(updatedHotel);
        }

        // DELETE: api/hoteles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHotel(int id)
        {
            var deleted = await _hotelService.DeleteAsync(id);
            
            if (!deleted)
            {
                return NotFound(new { message = $"Hotel con ID {id} no encontrado" });
            }

            return NoContent();
        }
    }
}