// Controllers/Client/ReservasController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Models.DTOs.Reserva;
using Hotel_chain.Models.Entities;
using Newtonsoft.Json;

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

        // ========== FLUJO DE 4 PASOS ==========

        // PASO 1: Iniciar Reserva - Huéspedes y Habitación
        public async Task<IActionResult> IniciarReserva(int habitacionId)
        {
            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            if (habitacion == null)
            {
                TempData["Error"] = "Habitación no encontrada";
                return RedirectToAction("Index", "Home");
            }

            // Guardar habitación en TempData para los siguientes pasos
            TempData["HabitacionId"] = habitacionId;
            TempData["HotelNombre"] = habitacion.Hotel?.Nombre;
            TempData["HabitacionNumero"] = habitacion.NumeroHabitacion;
            TempData.Keep();

            ViewBag.Habitacion = habitacion;
            return View("Paso1");
        }

        // POST PASO 1: Guardar número de huéspedes
        [HttpPost]
        public IActionResult GuardarPaso1(int numeroHuespedes)
        {
            if (numeroHuespedes < 1 || numeroHuespedes > 10)
            {
                TempData["Error"] = "El número de huéspedes debe estar entre 1 y 10";
                return RedirectToAction("IniciarReserva", new { habitacionId = TempData["HabitacionId"] });
            }

            TempData["NumeroHuespedes"] = numeroHuespedes;
            TempData.Keep();
            
            return RedirectToAction("Paso2");
        }

        // PASO 2: Seleccionar Fechas
        public async Task<IActionResult> Paso2()
        {
            if (TempData["HabitacionId"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var habitacionId = (int)TempData["HabitacionId"];
            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            
            ViewBag.Habitacion = habitacion;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            TempData.Keep();

            return View();
        }

        // POST PASO 2: Guardar fechas
        [HttpPost]
        public async Task<IActionResult> GuardarPaso2(DateTime fechaInicio, DateTime fechaFin)
        {
            var habitacionId = (int)TempData["HabitacionId"];

            if (fechaInicio < DateTime.Today)
            {
                TempData["Error"] = "La fecha de inicio no puede ser anterior a hoy";
                TempData.Keep();
                return RedirectToAction("Paso2");
            }

            if (fechaFin <= fechaInicio)
            {
                TempData["Error"] = "La fecha de fin debe ser posterior a la fecha de inicio";
                TempData.Keep();
                return RedirectToAction("Paso2");
            }

            // Verificar disponibilidad
            var disponible = await _reservaService.IsHabitacionDisponibleAsync(habitacionId, fechaInicio, fechaFin);
            if (!disponible)
            {
                TempData["Error"] = "La habitación no está disponible en las fechas seleccionadas";
                TempData.Keep();
                return RedirectToAction("Paso2");
            }

            TempData["FechaInicio"] = fechaInicio.ToString("yyyy-MM-dd");
            TempData["FechaFin"] = fechaFin.ToString("yyyy-MM-dd");
            TempData.Keep();

            return RedirectToAction("Paso3");
        }

        // PASO 3: Confirmar Habitación y Tarifa
        public async Task<IActionResult> Paso3()
        {
            if (TempData["HabitacionId"] == null || TempData["FechaInicio"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var habitacionId = (int)TempData["HabitacionId"];
            var fechaInicio = DateTime.Parse(TempData["FechaInicio"].ToString());
            var fechaFin = DateTime.Parse(TempData["FechaFin"].ToString());

            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacionId, fechaInicio, fechaFin);
            var numeroNoches = (fechaFin - fechaInicio).Days;

            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            TempData.Keep();

            return View();
        }

        // POST PASO 3: Ir a paso 4 (monto total)
        [HttpPost]
        public IActionResult GuardarPaso3(string solicitudesEspeciales)
        {
            TempData["SolicitudesEspeciales"] = solicitudesEspeciales;
            TempData.Keep();
            
            return RedirectToAction("Paso4");
        }

        // PASO 4: Monto Total y Confirmación
        public async Task<IActionResult> Paso4()
        {
            if (TempData["HabitacionId"] == null || TempData["FechaInicio"] == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var habitacionId = (int)TempData["HabitacionId"];
            var fechaInicio = DateTime.Parse(TempData["FechaInicio"].ToString());
            var fechaFin = DateTime.Parse(TempData["FechaFin"].ToString());

            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacionId, fechaInicio, fechaFin);
            var numeroNoches = (fechaFin - fechaInicio).Days;

            // Verificar si el usuario está logueado
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            var isLoggedIn = !string.IsNullOrEmpty(usuarioIdStr);

            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            ViewBag.SolicitudesEspeciales = TempData["SolicitudesEspeciales"];
            ViewBag.IsLoggedIn = isLoggedIn;
            TempData.Keep();

            return View();
        }

        // POST PASO 4: Confirmar Reserva (solo si está logueado)
        [HttpPost]
        public async Task<IActionResult> ConfirmarReserva()
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión para confirmar la reserva";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var reservaDto = new ReservaCreateDto
                {
                    UsuarioId = int.Parse(usuarioIdStr),
                    HabitacionId = (int)TempData["HabitacionId"],
                    FechaInicio = DateTime.Parse(TempData["FechaInicio"].ToString()),
                    FechaFin = DateTime.Parse(TempData["FechaFin"].ToString()),
                    NumeroHuespedes = (int)TempData["NumeroHuespedes"],
                    SolicitudesEspeciales = TempData["SolicitudesEspeciales"]?.ToString()
                };

                var reserva = await _reservaService.CreateAsync(reservaDto);
                
                // Limpiar TempData
                TempData.Clear();
                
                TempData["Success"] = $"¡Reserva creada exitosamente! Número de reserva: {reserva.NumeroReserva}";
                return RedirectToAction("MisReservas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al confirmar reserva");
                TempData["Error"] = ex.Message;
                TempData.Keep();
                return RedirectToAction("Paso4");
            }
        }

        // ========== MÉTODOS EXISTENTES ==========

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