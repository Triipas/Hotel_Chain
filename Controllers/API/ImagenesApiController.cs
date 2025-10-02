// Controllers/API/ImagenesApiController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Common;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers.API
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagenesApiController : ControllerBase
    {
        private readonly IS3Service _s3Service;
        private readonly AppDbContext _context;
        private readonly ILogger<ImagenesApiController> _logger;

        public ImagenesApiController(
            IS3Service s3Service,
            AppDbContext context,
            ILogger<ImagenesApiController> logger)
        {
            _s3Service = s3Service;
            _context = context;
            _logger = logger;
        }

        // POST: api/imagenes/hotel/5
        [HttpPost("hotel/{hotelId}")]
        public async Task<ActionResult<ApiResponse<List<Imagen>>>> UploadHotelImages(
            int hotelId,
            [FromForm] List<IFormFile> files)
        {
            try
            {
                if (files == null || !files.Any())
                {
                    return BadRequest(ApiResponse<List<Imagen>>.ErrorResult("No se proporcionaron archivos"));
                }

                // Verificar que el hotel existe
                var hotel = await _context.Hoteles.FindAsync(hotelId);
                if (hotel == null)
                {
                    return NotFound(ApiResponse<List<Imagen>>.ErrorResult($"Hotel con ID {hotelId} no encontrado"));
                }

                var imagenes = new List<Imagen>();

                foreach (var file in files)
                {
                    try
                    {
                        // Subir a S3
                        var folder = $"hoteles/{hotelId}";
                        var fileUrl = await _s3Service.UploadFileAsync(file, folder);

                        // Crear registro en BD
                        var imagen = new Imagen
                        {
                            NombreArchivo = Path.GetFileName(fileUrl),
                            UrlS3 = fileUrl,
                            TipoMime = file.ContentType,
                            Tamano = file.Length,
                            HotelId = hotelId,
                            FechaSubida = DateTime.UtcNow
                        };

                        _context.Imagenes.Add(imagen);
                        imagenes.Add(imagen);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al procesar archivo: {file.FileName}");
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(ApiResponse<List<Imagen>>.SuccessResult(
                    imagenes,
                    $"{imagenes.Count} imagen(es) subida(s) exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir imágenes del hotel");
                return StatusCode(500, ApiResponse<List<Imagen>>.ErrorResult(
                    "Error al subir las imágenes",
                    new List<string> { ex.Message }
                ));
            }
        }

        // POST: api/imagenes/habitacion/5
        [HttpPost("habitacion/{habitacionId}")]
        public async Task<ActionResult<ApiResponse<List<Imagen>>>> UploadHabitacionImages(
            int habitacionId,
            [FromForm] List<IFormFile> files)
        {
            try
            {
                if (files == null || !files.Any())
                {
                    return BadRequest(ApiResponse<List<Imagen>>.ErrorResult("No se proporcionaron archivos"));
                }

                // Verificar que la habitación existe
                var habitacion = await _context.Habitaciones.FindAsync(habitacionId);
                if (habitacion == null)
                {
                    return NotFound(ApiResponse<List<Imagen>>.ErrorResult($"Habitación con ID {habitacionId} no encontrada"));
                }

                var imagenes = new List<Imagen>();

                foreach (var file in files)
                {
                    try
                    {
                        // Subir a S3
                        var folder = $"habitaciones/{habitacionId}";
                        var fileUrl = await _s3Service.UploadFileAsync(file, folder);

                        // Crear registro en BD
                        var imagen = new Imagen
                        {
                            NombreArchivo = Path.GetFileName(fileUrl),
                            UrlS3 = fileUrl,
                            TipoMime = file.ContentType,
                            Tamano = file.Length,
                            HabitacionId = habitacionId,
                            FechaSubida = DateTime.UtcNow
                        };

                        _context.Imagenes.Add(imagen);
                        imagenes.Add(imagen);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"Error al procesar archivo: {file.FileName}");
                    }
                }

                await _context.SaveChangesAsync();

                return Ok(ApiResponse<List<Imagen>>.SuccessResult(
                    imagenes,
                    $"{imagenes.Count} imagen(es) subida(s) exitosamente"
                ));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al subir imágenes de la habitación");
                return StatusCode(500, ApiResponse<List<Imagen>>.ErrorResult(
                    "Error al subir las imágenes",
                    new List<string> { ex.Message }
                ));
            }
        }

        // DELETE: api/imagenes/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteImagen(int id)
        {
            try
            {
                var imagen = await _context.Imagenes.FindAsync(id);

                if (imagen == null)
                {
                    return NotFound(ApiResponse.ErrorResult($"Imagen con ID {id} no encontrada"));
                }

                // Eliminar de S3
                await _s3Service.DeleteFileAsync(imagen.UrlS3);

                // Eliminar de BD
                _context.Imagenes.Remove(imagen);
                await _context.SaveChangesAsync();

                return Ok(ApiResponse.SuccessResult("Imagen eliminada exitosamente"));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar imagen");
                return StatusCode(500, ApiResponse.ErrorResult(
                    "Error al eliminar la imagen",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/imagenes/hotel/5
        [HttpGet("hotel/{hotelId}")]
        public async Task<ActionResult<ApiResponse<List<Imagen>>>> GetHotelImages(int hotelId)
        {
            try
            {
                var imagenes = _context.Imagenes
                    .Where(i => i.HotelId == hotelId)
                    .OrderBy(i => i.FechaSubida)
                    .ToList();

                return Ok(ApiResponse<List<Imagen>>.SuccessResult(
                    imagenes,
                    "Imágenes obtenidas exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<Imagen>>.ErrorResult(
                    "Error al obtener imágenes",
                    new List<string> { ex.Message }
                ));
            }
        }

        // GET: api/imagenes/habitacion/5
        [HttpGet("habitacion/{habitacionId}")]
        public async Task<ActionResult<ApiResponse<List<Imagen>>>> GetHabitacionImages(int habitacionId)
        {
            try
            {
                var imagenes = _context.Imagenes
                    .Where(i => i.HabitacionId == habitacionId)
                    .OrderBy(i => i.FechaSubida)
                    .ToList();

                return Ok(ApiResponse<List<Imagen>>.SuccessResult(
                    imagenes,
                    "Imágenes obtenidas exitosamente"
                ));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<List<Imagen>>.ErrorResult(
                    "Error al obtener imágenes",
                    new List<string> { ex.Message }
                ));
            }
        }
    }
}