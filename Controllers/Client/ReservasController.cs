// Controllers/Client/ReservasController.cs
using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Models.DTOs.Reserva;
using Hotel_chain.Models.Entities;
using MercadoPago.Client.Preference;
using MercadoPago.Config;
using MercadoPago.Resource.Preference;
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

        // POST PASO 1: Guardar número de huéspedes Range(1, 10
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

            // 🔹 Guardar correctamente todos los datos
            TempData["HabitacionId"] = habitacionId;
            TempData["Adultos"] = adultos;
            TempData["Ninos"] = ninos;
            TempData["CantidadHabitaciones"] = cantidadHabitaciones;
            TempData["NumeroHuespedes"] = totalHuespedes;
            TempData.Keep();

            return RedirectToAction("Paso2");
        }
        // PASO 2: Seleccionar Fechas
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
            if (TempData["NumeroHuespedes"] != null)
            {
                TempData["NumeroHuespedes"] = TempData["NumeroHuespedes"];
            }
            TempData.Keep();

            return RedirectToAction("Paso3");
        }

        // PASO 3: Confirmar Habitación y Tarifa
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

            // 🔹 Obtener amenidades
            var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(habitacionId);

            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            ViewBag.Amenidades = amenidades; // <-- agregar a ViewBag
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

            // Obtener datos del usuario si está logueado
            Usuario usuario = null;
            if (isLoggedIn)
            {
                usuario = await _usuarioService.GetByIdAsync(int.Parse(usuarioIdStr));
            }

            // Pasar datos a la vista
            ViewBag.Habitacion = habitacion;
            ViewBag.FechaInicio = fechaInicio;
            ViewBag.FechaFin = fechaFin;
            ViewBag.NumeroNoches = numeroNoches;
            ViewBag.PrecioTotal = precioTotal;
            ViewBag.NumeroHuespedes = TempData["NumeroHuespedes"];
            ViewBag.SolicitudesEspeciales = TempData["SolicitudesEspeciales"];
            ViewBag.IsLoggedIn = isLoggedIn;
            ViewBag.Usuario = usuario; // Aquí guardamos el usuario completo

            TempData.Keep(); // Mantener TempData para futuros pasos

            return View();
        }

        // POST PASO 4: Confirmar Reserva (solo si está logueado)
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
                // 🔹 Obtener datos desde Session (si existen)
                var habitacionId = HttpContext.Session.GetInt32("HabitacionId") 
                                ?? (int?)TempData["HabitacionId"];
                var fechaInicioStr = HttpContext.Session.GetString("FechaInicio") 
                                    ?? TempData["FechaInicio"]?.ToString();
                var fechaFinStr = HttpContext.Session.GetString("FechaFin") 
                                ?? TempData["FechaFin"]?.ToString();

                if (habitacionId == null || fechaInicioStr == null || fechaFinStr == null)
                {
                    TempData["Error"] = "Datos de reserva incompletos. Intenta nuevamente.";
                    return RedirectToAction("Paso4");
                }

                var fechaInicio = DateTime.Parse(fechaInicioStr);
                var fechaFin = DateTime.Parse(fechaFinStr);
                var numeroNoches = (fechaFin - fechaInicio).Days;

                var numeroHuespedes = HttpContext.Session.GetInt32("NumeroHuespedes") 
                                    ?? (int?)TempData["NumeroHuespedes"] ?? 1;
                var adultos = HttpContext.Session.GetInt32("Adultos") 
                            ?? (int?)TempData["Adultos"] ?? 1;
                var ninos = HttpContext.Session.GetInt32("Ninos") 
                            ?? (int?)TempData["Ninos"] ?? 0;
                var solicitudes = HttpContext.Session.GetString("SolicitudesEspeciales") 
                                ?? TempData["SolicitudesEspeciales"]?.ToString();

                // Obtener usuario y habitación
                var usuario = await _usuarioService.GetByIdAsync(int.Parse(usuarioIdStr));
                var habitacion = await _habitacionService.GetByIdAsync(habitacionId.Value);
                var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacion.HabitacionId, fechaInicio, fechaFin);

                // Crear DTO
                var reservaDto = new ReservaCreateDto
                {
                    UsuarioId = usuario.UsuarioId,
                    HabitacionId = habitacion.HabitacionId,
                    HotelId = habitacion.HotelId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    NumeroNoches = numeroNoches,
                    NumeroHuespedes = numeroHuespedes,
                    GuestsAdults = adultos,
                    GuestsChildren = ninos,
                    GuestFirstName = usuario.Nombre,
                    GuestLastName = usuario.Apellido,
                    GuestEmail = usuario.Email,
                    GuestPhone = usuario.Telefono,
                    RoomRate = habitacion.PrecioNoche,
                    Subtotal = habitacion.PrecioBase,
                    Taxes = habitacion.PrecioImpuestos,
                    Currency = habitacion.Moneda ?? "PEN",
                    PrecioTotal = precioTotal,
                    PaymentMethod = PaymentMethod ?? "Mercado Pago",
                    SolicitudesEspeciales = solicitudes
                };

                // Guardar reserva
                var reserva = await _reservaService.CreateAsync(reservaDto);

                // Limpiar los datos usados
                TempData.Clear();
                HttpContext.Session.Remove("HabitacionId");
                HttpContext.Session.Remove("FechaInicio");
                HttpContext.Session.Remove("FechaFin");
                HttpContext.Session.Remove("NumeroHuespedes");
                HttpContext.Session.Remove("Adultos");
                HttpContext.Session.Remove("Ninos");
                HttpContext.Session.Remove("SolicitudesEspeciales");

                TempData["Success"] = $"¡Reserva creada exitosamente! Número de reserva: {reserva.NumeroReserva}";
                return RedirectToAction("MisReservas");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al confirmar reserva");
                TempData["Error"] = "Ocurrió un error al confirmar la reserva: " + ex.Message;
                TempData.Keep();
                return RedirectToAction("Paso4");
            }
        }

               [HttpPost]
public async Task<IActionResult> PagarConMercadoPago()
{
    try
    {
        // Verificar que existan datos en TempData
        if (TempData["HabitacionId"] == null || TempData["FechaInicio"] == null)
        {
            TempData["Error"] = "Datos de sesión incompletos";
            return RedirectToAction("Paso4");
        }

        // Mantener los TempData por si se necesitan en la siguiente vista
        TempData.Keep();

        // Configurar Access Token de Mercado Pago
        MercadoPagoConfig.AccessToken = "APP_USR-7414149299049294-101023-a6c8228e7ab8480545ec141f3615d304-2914738092";

        // Extraer valores de TempData
        var habitacionId = (int)TempData["HabitacionId"];
        var fechaInicio = DateTime.Parse(TempData["FechaInicio"].ToString());
        var fechaFin = DateTime.Parse(TempData["FechaFin"].ToString());

        // Guardar valores importantes en Session
        HttpContext.Session.SetInt32("HabitacionId", habitacionId);
        HttpContext.Session.SetString("FechaInicio", fechaInicio.ToString("yyyy-MM-dd"));
        HttpContext.Session.SetString("FechaFin", fechaFin.ToString("yyyy-MM-dd"));

        if (TempData["NumeroHuespedes"] != null)
            HttpContext.Session.SetInt32("NumeroHuespedes", (int)TempData["NumeroHuespedes"]);
        if (TempData["Adultos"] != null)
            HttpContext.Session.SetInt32("Adultos", (int)TempData["Adultos"]);
        if (TempData["Ninos"] != null)
            HttpContext.Session.SetInt32("Ninos", (int)TempData["Ninos"]);
        if (TempData["SolicitudesEspeciales"] != null)
            HttpContext.Session.SetString("SolicitudesEspeciales", TempData["SolicitudesEspeciales"].ToString());

        // Verificar usuario logueado
        var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
        if (string.IsNullOrEmpty(usuarioIdStr))
        {
            TempData["Error"] = "Debes iniciar sesión para continuar con el pago.";
            return RedirectToAction("Index", "Login");
        }

        // Obtener datos relacionados
        var habitacion = await _habitacionService.GetByIdAsync(habitacionId);
        var hotel = await _hotelService.GetByIdAsync(habitacion.HotelId);
        var usuario = await _usuarioService.GetByIdAsync(int.Parse(usuarioIdStr));
        var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacionId, fechaInicio, fechaFin);

        var nombreHabitacion = $"{hotel.Nombre} - Habitación {habitacion.NumeroHabitacion}";

        // ✅ URL base fija al puerto HTTPS correcto
        var baseUrl = "https://localhost:7194"; // Usa siempre este puerto en tu app local HTTPS

        // Crear la preferencia de pago
        var request = new PreferenceRequest
        {
            Items = new List<PreferenceItemRequest>
            {
                new PreferenceItemRequest
                {
                    Title = nombreHabitacion,
                    Quantity = 1,
                    CurrencyId = "PEN",
                    UnitPrice = (decimal)precioTotal
                }
            },
            Payer = new PreferencePayerRequest
            {
                Name = usuario.Nombre,
                Email = usuario.Email
            },
            BackUrls = new PreferenceBackUrlsRequest
            {
                Success = $"{baseUrl}/Reservas/Exito",
                Failure = $"{baseUrl}/Reservas/Error",
                Pending = $"{baseUrl}/Reservas/Pendiente"
            },
            AutoReturn = "approved",
            NotificationUrl = $"{baseUrl}/api/pagos/notificacion" // opcional si usas notificación IPN
        };

        var client = new PreferenceClient();
        var preference = await client.CreateAsync(request);

        // 🔍 Logs informativos
        _logger.LogInformation("✅ Preferencia creada correctamente");
        _logger.LogInformation($"🆔 preference_id: {preference.Id}");
        _logger.LogInformation($"🔗 URL de pago: {preference.InitPoint}");
        _logger.LogInformation($"🔙 Success URL: {request.BackUrls.Success}");

        // Redirigir al checkout de Mercado Pago
        return Redirect(preference.InitPoint);
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al generar el pago con Mercado Pago");
        TempData["Error"] = "Ocurrió un error al iniciar el pago. Intenta nuevamente.";
        TempData.Keep();
        return RedirectToAction("Paso4");
    }
}


        // Método alternativo para manejar la ruta /Exito (por si Mercado Pago usa esta)

          [HttpGet]
public async Task<IActionResult> Exito(string collection_id, string collection_status, string payment_id, string status, string preference_id)
{
    try
    {
        // 🔍 1. Verificar que el pago fue aprobado
        if (status != "approved" && collection_status != "approved")
        {
            TempData["Error"] = "El pago no fue aprobado.";
            return RedirectToAction("Error");
        }

        // 🧠 2. Recuperar los datos de la reserva desde la sesión
        int? habitacionId = HttpContext.Session.GetInt32("HabitacionId");
        string fechaInicioStr = HttpContext.Session.GetString("FechaInicio");
        string fechaFinStr = HttpContext.Session.GetString("FechaFin");
        int? usuarioId = int.Parse(HttpContext.Session.GetString("UsuarioId") ?? "0");

        if (habitacionId == null || string.IsNullOrEmpty(fechaInicioStr) || string.IsNullOrEmpty(fechaFinStr) || usuarioId == 0)
        {
            TempData["Error"] = "No se pudieron recuperar los datos de la reserva.";
            return RedirectToAction("Error");
        }

        DateTime fechaInicio = DateTime.Parse(fechaInicioStr);
        DateTime fechaFin = DateTime.Parse(fechaFinStr);
        var numeroNoches = (fechaFin - fechaInicio).Days;

        // Obtener datos adicionales desde la sesión
        int numeroHuespedes = HttpContext.Session.GetInt32("NumeroHuespedes") ?? 1;
        int adultos = HttpContext.Session.GetInt32("Adultos") ?? 1;
        int ninos = HttpContext.Session.GetInt32("Ninos") ?? 0;
        string solicitudes = HttpContext.Session.GetString("SolicitudesEspeciales");

        // Obtener datos desde servicios
        var usuario = await _usuarioService.GetByIdAsync(usuarioId.Value);
        var habitacion = await _habitacionService.GetByIdAsync(habitacionId.Value);
        var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacion.HabitacionId, fechaInicio, fechaFin);

        // 💾 3. Registrar la reserva en la base de datos usando ReservaCreateDto
        var reservaDto = new ReservaCreateDto
        {
            UsuarioId = usuario.UsuarioId,
            HabitacionId = habitacion.HabitacionId,
            HotelId = habitacion.HotelId,
            FechaInicio = fechaInicio,
            FechaFin = fechaFin,
            NumeroNoches = numeroNoches,
            NumeroHuespedes = numeroHuespedes,
            GuestsAdults = adultos,
            GuestsChildren = ninos,
            GuestFirstName = usuario.Nombre,
            GuestLastName = usuario.Apellido,
            GuestEmail = usuario.Email,
            GuestPhone = usuario.Telefono,
            RoomRate = habitacion.PrecioNoche,
            Subtotal = habitacion.PrecioBase,
            Taxes = habitacion.PrecioImpuestos,
            Currency = habitacion.Moneda ?? "PEN",
            PrecioTotal = precioTotal,
            PaymentMethod = "Mercado Pago",
            SolicitudesEspeciales = solicitudes
        };

        var reserva = await _reservaService.CreateAsync(reservaDto);

        // 🔔 4. Limpia la sesión después del registro
        HttpContext.Session.Remove("HabitacionId");
        HttpContext.Session.Remove("FechaInicio");
        HttpContext.Session.Remove("FechaFin");
        HttpContext.Session.Remove("NumeroHuespedes");
        HttpContext.Session.Remove("Adultos");
        HttpContext.Session.Remove("Ninos");
        HttpContext.Session.Remove("SolicitudesEspeciales");

        // ✅ 5. Redirigir a la página de confirmación
        _logger.LogInformation($"✅ Reserva creada exitosamente: {reserva.NumeroReserva}");
        return RedirectToAction("Confirmacion", new { reservaId = reserva.ReservaId });
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "Error al procesar el pago exitoso");
        TempData["Error"] = "Ocurrió un error al procesar el pago exitoso.";
        return RedirectToAction("PagoFallido");
    }
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

            // 🔹 Traer las amenidades de la habitación
            var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(reserva.HabitacionId);

            // Asegurarse de que no sea null para evitar errores en la vista
            ViewBag.Amenidades = amenidades?.ToList() ?? new List<HabitacionAmenidad>();

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

            // 1️⃣ Verificar que la reserva existe y está completada
            var reserva = await _reservaService.GetByIdAsync(request.ReservaId);
            if (reserva == null) return NotFound("Reserva no encontrada");
            if (reserva.Estado.ToLower() != "completada")
                return BadRequest("Solo se puede calificar reservas completadas");

            // 2️⃣ Verificar que el usuario no haya calificado esta reserva
            var existeResena = await _resenaService.ExisteResenaAsync(request.ReservaId, usuarioId);
            if (existeResena)
                return BadRequest("Ya has calificado esta reserva");

            // 3️⃣ Crear reseña
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

            // 4️⃣ Actualizar promedio del hotel
            var hotel = await _hotelService.GetByIdAsync(request.HotelId);
            if (hotel != null)
            {
                hotel.Calificacion = await _resenaService.GetPromedioCalificacionAsync(request.HotelId);
                await _hotelService.UpdateAsync(hotel.HotelId, hotel);
            }

            return Ok();
        }
        [HttpGet]
        public async Task<IActionResult> PagoExitoso(string payment_id, string status, string preference_id)
        {
            try
            {
                _logger.LogInformation($"PagoExitoso llamado - PaymentID: {payment_id}, Status: {status}");

                // Verificar que el pago fue aprobado
                if (status != "approved")
                {
                    TempData["Error"] = "El pago no fue aprobado. Estado: " + status;
                    return RedirectToAction("Paso4");
                }

                // Recuperar datos de TempData
                var habitacionId = TempData["HabitacionId"] as int?;
                var fechaInicioStr = TempData["FechaInicio"] as string;
                var fechaFinStr = TempData["FechaFin"] as string;
                var numeroHuespedes = TempData["NumeroHuespedes"] as int?;
                var adultos = TempData["Adultos"] as int?;
                var ninos = TempData["Ninos"] as int?;
                var solicitudes = TempData["SolicitudesEspeciales"] as string;

                var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");

                // Validar datos esenciales
                if (!habitacionId.HasValue || string.IsNullOrEmpty(fechaInicioStr) ||
                    string.IsNullOrEmpty(fechaFinStr) || string.IsNullOrEmpty(usuarioIdStr))
                {
                    TempData["Error"] = "Datos de sesión incompletos. Por favor, inicia el proceso nuevamente.";
                    return RedirectToAction("Index", "Hotel");
                }

                // Convertir fechas
                var fechaInicio = DateTime.Parse(fechaInicioStr);
                var fechaFin = DateTime.Parse(fechaFinStr);
                var numeroNoches = (fechaFin - fechaInicio).Days;

                // Obtener datos desde servicios
                var usuario = await _usuarioService.GetByIdAsync(int.Parse(usuarioIdStr));
                var habitacion = await _habitacionService.GetByIdAsync(habitacionId.Value);
                var precioTotal = await _reservaService.CalcularPrecioTotalAsync(habitacion.HabitacionId, fechaInicio, fechaFin);

                // Crear DTO de reserva
                var reservaDto = new ReservaCreateDto
                {
                    UsuarioId = usuario.UsuarioId,
                    HabitacionId = habitacion.HabitacionId,
                    HotelId = habitacion.HotelId,
                    FechaInicio = fechaInicio,
                    FechaFin = fechaFin,
                    NumeroNoches = numeroNoches,
                    NumeroHuespedes = numeroHuespedes ?? 1,
                    GuestsAdults = adultos ?? 1,
                    GuestsChildren = ninos ?? 0,
                    GuestFirstName = usuario.Nombre,
                    GuestLastName = usuario.Apellido,
                    GuestEmail = usuario.Email,
                    GuestPhone = usuario.Telefono,
                    RoomRate = habitacion.PrecioNoche,
                    Subtotal = habitacion.PrecioBase,
                    Taxes = habitacion.PrecioImpuestos,
                    Currency = habitacion.Moneda ?? "PEN",
                    PrecioTotal = precioTotal,
                    PaymentMethod = "Mercado Pago",
                    SolicitudesEspeciales = solicitudes
                };

                // Crear la reserva
                var reserva = await _reservaService.CreateAsync(reservaDto);

                // Limpiar datos temporales
                TempData.Clear();

                _logger.LogInformation($"✅ Reserva creada exitosamente tras pago: {reserva.NumeroReserva}");

                // Redirigir a la página de confirmación
                return RedirectToAction("Confirmacion", new { reservaId = reserva.ReservaId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar PagoExitoso");
                TempData["Error"] = "Error al procesar la reserva: " + ex.Message;
                TempData.Keep();
                return RedirectToAction("Paso4");
            }
        }



        // Página de confirmación de reserva exitosa
        [HttpGet]
        public async Task<IActionResult> Confirmacion(int reservaId)
        {
            var usuarioIdStr = HttpContext.Session.GetString("UsuarioId");
            if (string.IsNullOrEmpty(usuarioIdStr))
            {
                TempData["Error"] = "Debes iniciar sesión";
                return RedirectToAction("Index", "Login");
            }

            var reserva = await _reservaService.GetByIdAsync(reservaId);
            
            if (reserva == null)
            {
                TempData["Error"] = "No se encontró la reserva";
                return RedirectToAction("Index", "Hotel");
            }

            // Verificar que la reserva pertenezca al usuario
            if (reserva.UsuarioId != int.Parse(usuarioIdStr))
            {
                TempData["Error"] = "No tienes permiso para ver esta reserva";
                return RedirectToAction("MisReservas");
            }

            return View(reserva);
        }

        // Manejo de pagos fallidos
        [HttpGet]
        public IActionResult PagoFallido(string payment_id, string status)
        {
            _logger.LogWarning($"Pago fallido - PaymentID: {payment_id}, Status: {status}");
            TempData["Error"] = "El pago no pudo ser procesado. Por favor, intenta nuevamente.";
            TempData.Keep();
            return RedirectToAction("Paso4");
        }

        // Manejo de pagos pendientes
        [HttpGet]
        public IActionResult PagoPendiente(string payment_id, string status)
        {
            _logger.LogInformation($"Pago pendiente - PaymentID: {payment_id}, Status: {status}");
            TempData["Info"] = "Tu pago está pendiente de confirmación. Te notificaremos cuando se procese.";
            return RedirectToAction("MisReservas");
        }    


// DTO para recibir JSON
public class CalificarRequest
{
    public int ReservaId { get; set; }
    public int HotelId { get; set; }
    public decimal Calificacion { get; set; }
    public string Comentario { get; set; } = null!;
}


    }
}