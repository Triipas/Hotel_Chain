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

        private readonly IHabitacionAmenidadService _habitacionAmenidadService;
        private readonly IUsuarioService _usuarioService;
        private readonly IResenaService _resenaService;

        public ReservasController(
            IReservaService reservaService,
            IHotelService hotelService,
            IHabitacionService habitacionService,
            IHabitacionAmenidadService habitacionAmenidadService,
            IUsuarioService usuarioService,
            IResenaService resenaService,
            ILogger<ReservasController> logger)
        {
            _reservaService = reservaService;
            _hotelService = hotelService;
            _habitacionService = habitacionService;
            _habitacionAmenidadService = habitacionAmenidadService;
            _usuarioService = usuarioService;
            _resenaService = resenaService;
            _logger = logger;
        }

        public async Task<IActionResult> IniciarReserva(int habitacionId)
        {
            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            if (habitacion == null)
            {
                TempData["Error"] = "Habitación no encontrada";
                return RedirectToAction("Index", "Home");
            }

            TempData["HabitacionId"] = habitacionId;
            TempData["HotelNombre"] = habitacion.Hotel?.Nombre;
            TempData["HabitacionNumero"] = habitacion.NumeroHabitacion;
            TempData.Keep();

            ViewBag.Habitacion = habitacion;
            return View("Paso1");
        }

        [HttpPost]
        public async Task<IActionResult> GuardarPaso1(int habitacionId, int adultos, int ninos, int cantidadHabitaciones)
        {
            Console.WriteLine($"Adultos: {adultos}, Niños: {ninos}, Cant: {cantidadHabitaciones}");

            int totalHuespedes = adultos + ninos;

            if (totalHuespedes < 1 || totalHuespedes > 10)
            {
                TempData["Error"] = "⚠️ El número de huéspedes debe estar entre 1 y 10";
                return RedirectToAction("Paso1", new { habitacionId });
            }

            TempData["HabitacionId"] = habitacionId;
            TempData["Adultos"] = adultos;
            TempData["Ninos"] = ninos;
            TempData["CantidadHabitaciones"] = cantidadHabitaciones;
            TempData["NumeroHuespedes"] = totalHuespedes;
            TempData.Keep();

            return RedirectToAction("Paso2");
        }

        public async Task<IActionResult> Paso2()
        {
            if (TempData["HabitacionId"] == null)
                return RedirectToAction("Index", "Home");

            int habitacionId = (int)TempData["HabitacionId"];
            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);

            ViewBag.Habitacion = habitacion;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            TempData.Keep();

            return View();
        }

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

            var disponible = await _reservaService.IsHabitacionDisponibleAsync(habitacionId, fechaInicio, fechaFin);
            if (!disponible)
            {
                TempData["Error"] = "La habitación no está disponible en las fechas seleccionadas";
                TempData.Keep();
                return RedirectToAction("Paso2");
            }

            TempData["FechaInicio"] = fechaInicio.ToString("yyyy-MM-dd");
            TempData["FechaFin"] = fechaFin.ToString("yyyy-MM-dd");
            if (TempData["NumeroHuespedes"] != null)
            {
                TempData["NumeroHuespedes"] = TempData["NumeroHuespedes"];
            }
            TempData.Keep();

            return RedirectToAction("Paso3");
        }

        public async Task<IActionResult> Paso3()
        {
            if (TempData["HabitacionId"] == null || TempData["FechaInicio"] == null)
                return RedirectToAction("Index", "Home");

            var habitacionId = (int)TempData["HabitacionId"];
            var fechaInicio = DateTime.Parse(TempData["FechaInicio"].ToString());
            var fechaFin = DateTime.Parse(TempData["FechaFin"].ToString());

            var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
            var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacionId, fechaInicio, fechaFin);
            var numeroNoches = (fechaFin - fechaInicio).Days;

            var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(habitacionId);

            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            ViewBag.Amenidades = amenidades;
            TempData.Keep();

            return View();
        }

        [HttpPost]
        public IActionResult GuardarPaso3(string solicitudesEspeciales)
        {
            TempData["SolicitudesEspeciales"] = solicitudesEspeciales;
            TempData.Keep();

            return RedirectToAction("Paso4");
        }

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

            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            var isLoggedIn = !string.IsNullOrEmpty(usuarioIdStr);

            Usuario usuario = null;
            if (isLoggedIn)
            {
                usuario = await _usuarioService.GetByIdAsync(int.Parse(usuarioIdStr));
            }

            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            ViewBag.SolicitudesEspeciales = TempData["SolicitudesEspeciales"];
            ViewBag.IsLoggedIn = isLoggedIn;
            ViewBag.Usuario = usuario;

            TempData.Keep();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmarReserva(string PaymentMethod)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión para confirmar la reserva";
                return RedirectToAction("Index", "Login");
            }

            try
            {
                var usuario = await _usuarioService.GetByIdAsync(int.Parse(usuarioIdStr));
                var habitacion = await _habitacionService.GetByIdAsync((int)TempData["HabitacionId"]);

                var fechaInicio = DateTime.Parse(TempData["FechaInicio"].ToString());
                var fechaFin = DateTime.Parse(TempData["FechaFin"].ToString());
                var numeroNoches = (fechaFin - fechaInicio).Days;
                var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacion.HabitacionId, fechaInicio, fechaFin);

                var reservaDto = new ReservaCreateDto
                {
                    UsuarioId = usuario.UsuarioId,
                    HabitacionId = habitacion.HabitacionId,
                    HotelId = habitacion.HotelId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    NumeroNoches = numeroNoches,
                    NumeroHuespedes = (int)TempData["NumeroHuespedes"],
                    GuestsAdults = (int)TempData["Adultos"],
                    GuestsChildren = (int)TempData["Ninos"],
                    GuestFirstName = usuario.Nombre,
                    GuestLastName = usuario.Apellido,
                    GuestEmail = usuario.Email,
                    GuestPhone = usuario.Telefono,
                    RoomRate = habitacion.PrecioNoche,
                    Subtotal = habitacion.PrecioBase,
                    Taxes = habitacion.PrecioImpuestos,
                    Currency = habitacion.Moneda,
                    PrecioTotal = precioTotal,
                    PaymentMethod = PaymentMethod,
                    SolicitudesEspeciales = TempData["SolicitudesEspeciales"]?.ToString()
                };

                var reserva = await _reservaService.CreateAsync(reservaDto);

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

        public async Task<IActionResult> BuscarDisponibilidad()
        {
            ViewBag.Hoteles = await _hotelService.GetAllAsync();
            return View();
        }

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

            if (reserva.UsuarioId != int.Parse(usuarioIdStr))
            {
                TempData["Error"] = "No tienes permiso para ver esta reserva";
                return RedirectToAction("MisReservas");
            }

            var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(reserva.HabitacionId);

            ViewBag.Amenidades = amenidades?.ToList() ?? new List<HabitacionAmenidad>();

            return View(reserva);
        }

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


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<IActionResult> CalificarEstancia([FromBody] CalificarRequest request)
{
    var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
    if (string.IsNullOrEmpty(usuarioIdStr)) return Unauthorized();

    int usuarioId = int.Parse(usuarioIdStr);

    var reserva = await _reservaService.GetByIdAsync(request.ReservaId);
    if (reserva == null) return NotFound("Reserva no encontrada");
    if (reserva.Estado.ToLower() != "completada") 
        return BadRequest("Solo se puede calificar reservas completadas");

    var existeResena = await _resenaService.ExisteResenaAsync(request.ReservaId, usuarioId);
    if (existeResena)
        return BadRequest("Ya has calificado esta reserva");

    await _resenaService.CreateAsync(new Reseña
    {
        ReservaId = request.ReservaId,
        HotelId = request.HotelId,
        UsuarioId = usuarioId,
        Calificacion = request.Calificacion,
        Comentario = request.Comentario,
        CreatedAt = DateTime.UtcNow,
        UpdatedAt = DateTime.UtcNow
    });

    var hotel = await _hotelService.GetByIdAsync(request.HotelId);
    if (hotel != null)
    {
        hotel.Calificacion = await _resenaService.GetPromedioCalificacionAsync(request.HotelId);
        await _hotelService.UpdateAsync(hotel.HotelId, hotel);
    }

    return Ok();
}

public class CalificarRequest
{
    public int ReservaId { get; set; }
    public int HotelId { get; set; }
    public decimal Calificacion { get; set; }
    public string Comentario { get; set; } = null!;
}


    }
}