// Controllers/Client/ReservasController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Models.DTOs.Reserva;
using Hotel_chain.Models.Entities;

namespace Hotel_chain.Controllers.Client
{
    public class ReservasController : Controller
    {
        private readonly IReservaService _reservaService;
        private readonly IHotelService _hotelService;
        private readonly IHabitacionService _habitacionService;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(
            IReservaService reservaService,
            IHotelService hotelService,
            IHabitacionService habitacionService,
            ILogger<ReservasController> logger)
        {
            _reservaService = reservaService;
            _hotelService = hotelService;
            _habitacionService = habitacionService;
            _logger = logger;
        }

        // GET: /Reservas/BuscarDisponibilidad
        public async Task<IActionResult> BuscarDisponibilidad()
        {
            ViewBag.Hoteles = await _hotelService.GetAllAsync();
            return View();
        }

        // POST: /Reservas/BuscarDisponibilidad
        [HttpPost]
        public async Task<IActionResult> BuscarDisponibilidad(DisponibilidadSearchDto searchDto)
        {
            ViewBag.Hoteles = await _hotelService.GetAllAsync();

            if (!ModelState.IsValid)
            {
                return View(searchDto);
            }

            try
            {
                var habitacionesDisponibles = await _reservaService.GetHabitacionesDisponiblesAsync(
                    searchDto.HotelId,
                    searchDto.FechaInicio,
                    searchDto.FechaFin,
                    searchDto.NumeroHuespedes
                );

                var hotel = await _hotelService.GetByIdAsync(searchDto.HotelId);
                
                ViewBag.Hotel = hotel;
                ViewBag.FechaInicio = searchDto.FechaInicio;
                ViewBag.FechaFin = searchDto.FechaFin;
                ViewBag.NumeroHuespedes = searchDto.NumeroHuespedes;
                ViewBag.NumeroNoches = (searchDto.FechaFin - searchDto.FechaInicio).Days;

                return View("ResultadosDisponibilidad", habitacionesDisponibles);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al buscar disponibilidad");
                ModelState.AddModelError("", "Error al buscar disponibilidad: " + ex.Message);
                return View(searchDto);
            }
        }

        // GET: /Reservas/Crear?habitacionId=1&fechaInicio=...&fechaFin=...&huespedes=2
        public async Task<IActionResult> Crear(int habitacionId, DateTime fechaInicio, DateTime fechaFin, int huespedes)
        {
            // Verificar que el usuario esté logueado
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión para hacer una reserva";
                return RedirectToAction("Index", "Login");
            }

            // Obtener la habitación
            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            if (habitacion == null)
            {
                TempData["Error"] = "Habitación no encontrada";
                return RedirectToAction("BuscarDisponibilidad");
            }

            // Verificar disponibilidad
            var disponible = await _reservaService.IsHabitacionDisponibleAsync(habitacionId, fechaInicio, fechaFin);
            if (!disponible)
            {
                TempData["Error"] = "La habitación ya no está disponible en estas fechas";
                return RedirectToAction("BuscarDisponibilidad");
            }

            // Calcular datos de la reserva
            var numeroNoches = (fechaFin - fechaInicio).Days;
            var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacionId, fechaInicio, fechaFin);

            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroHuespedes = huespedes;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;

            return View();
        }

        // POST: /Reservas/Crear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Crear(ReservaCreateDto reservaDto)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Sesión expirada. Por favor inicia sesión nuevamente";
                return RedirectToAction("Index", "Login");
            }

            reservaDto.UsuarioId = int.Parse(usuarioIdStr);

            if (!ModelState.IsValid)
            {
                var habitacion = await _habitacionService.GetByIdAsync(reservaDto.HabitacionId);
                ViewBag.Habitacion = habitacion;
                return View(reservaDto);
            }

            try
            {
                var reserva = await _reservaService.CreateAsync(reservaDto);
                TempData["Success"] = $"¡Reserva creada exitosamente! Número de reserva: {reserva.NumeroReserva}";
                return RedirectToAction("MisReservas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reserva");
                ModelState.AddModelError("", ex.Message);
                
                var habitacion = await _habitacionService.GetByIdAsync(reservaDto.HabitacionId);
                ViewBag.Habitacion = habitacion;
                return View(reservaDto);
            }
        }

        // GET: /Reservas/MisReservas
        public async Task<IActionResult> MisReservas()
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión para ver tus reservas";
                return RedirectToAction("Index", "Login");
            }

            var usuarioId = int.Parse(usuarioIdStr);
            var reservas = await _reservaService.GetByUsuarioIdAsync(usuarioId);

            return View(reservas);
        }

        // GET: /Reservas/Detalle/5
        public async Task<IActionResult> Detalle(int id)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Index", "Login");
            }

            var reserva = await _reservaService.GetByIdAsync(id);
            if (reserva == null)
            {
                TempData["Error"] = "Reserva no encontrada";
                return RedirectToAction("MisReservas");
            }

            // Verificar que la reserva pertenezca al usuario
            if (reserva.UsuarioId != int.Parse(usuarioIdStr))
            {
                TempData["Error"] = "No tienes permiso para ver esta reserva";
                return RedirectToAction("MisReservas");
            }

            return View(reserva);
        }

        // POST: /Reservas/Cancelar/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancelar(int id, string motivo)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var reserva = await _reservaService.GetByIdAsync(id);
                if (reserva == null)
                {
                    TempData["Error"] = "Reserva no encontrada";
                    return RedirectToAction("MisReservas");
                }

                // Verificar que la reserva pertenezca al usuario
                if (reserva.UsuarioId != int.Parse(usuarioIdStr))
                {
                    TempData["Error"] = "No tienes permiso para cancelar esta reserva";
                    return RedirectToAction("MisReservas");
                }

                await _reservaService.CancelarReservaAsync(id, motivo ?? "Cancelada por el usuario");
                TempData["Success"] = "Reserva cancelada exitosamente";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar reserva");
                TempData["Error"] = ex.Message;
            }

            return RedirectToAction("MisReservas");
        }
    }
}