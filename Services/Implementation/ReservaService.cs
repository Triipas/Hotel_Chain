// Services/Implementation/ReservaService.cs
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Data;
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Reserva;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class ReservaService : IReservaService
    {
        private readonly AppDbContext _context;
        private readonly ILogger<ReservaService> _logger;

        public ReservaService(AppDbContext context, ILogger<ReservaService> logger)
        {
            _context = context;
            _logger = logger;
        }

        #region Consultas

        public async Task<IEnumerable<Reserva>> GetAllAsync()
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                    .ThenInclude(h => h.Hotel)
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        public async Task<Reserva?> GetByIdAsync(int id)
        {
       return await _context.Reservas
    .Include(r => r.Habitacion)
        .ThenInclude(h => h.Hotel)
            .ThenInclude(h => h.Imagenes)
    .Include(r => r.Habitacion)
        .ThenInclude(h => h.Imagenes)
    .Include(r => r.Usuario)
    .FirstOrDefaultAsync(r => r.ReservaId == id);
        }

        public async Task<Reserva?> GetByNumeroReservaAsync(string numeroReserva)
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                    .ThenInclude(h => h.Hotel)
                .FirstOrDefaultAsync(r => r.NumeroReserva == numeroReserva);
        }

        public async Task<IEnumerable<Reserva>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _context.Reservas
    .Include(r => r.Habitacion)
        .ThenInclude(h => h.Hotel)
            .ThenInclude(h => h.Imagenes) // <-- cargar imágenes del hotel
    .Include(r => r.Habitacion)
        .ThenInclude(h => h.Imagenes)   // <-- cargar imágenes de la habitación si quieres
    .Include(r => r.Usuario) // cargar datos del usuario
    .Where(r => r.UsuarioId == usuarioId)
    .OrderByDescending(r => r.FechaCreacion)
    .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> GetByHabitacionIdAsync(int habitacionId)
        {
            return await _context.Reservas
                .Include(r => r.Usuario)
                .Where(r => r.HabitacionId == habitacionId)
                .OrderByDescending(r => r.FechaInicio)
                .ToListAsync();
        }

        public async Task<IEnumerable<Reserva>> SearchAsync(string? estado, string? estadoPago, DateTime? fechaInicio, DateTime? fechaFin)
        {
            var query = _context.Reservas
                .Include(r => r.Usuario)
                .Include(r => r.Habitacion)
                    .ThenInclude(h => h.Hotel)
                .AsQueryable();

            if (!string.IsNullOrEmpty(estado))
                query = query.Where(r => r.Estado == estado);

            if (!string.IsNullOrEmpty(estadoPago))
                query = query.Where(r => r.EstadoPago == estadoPago);

            if (fechaInicio.HasValue)
                query = query.Where(r => r.FechaInicio >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(r => r.FechaFin <= fechaFin.Value);

            return await query
                .OrderByDescending(r => r.FechaCreacion)
                .ToListAsync();
        }

        #endregion

        #region Disponibilidad

        public async Task<bool> IsHabitacionDisponibleAsync(int habitacionId, DateTime fechaInicio, DateTime fechaFin, int? excludeReservaId = null)
        {
            var query = _context.Reservas
                .Where(r => r.HabitacionId == habitacionId &&
                           r.Estado != "cancelada" &&
                           r.Estado != "completada" &&
                           ((r.FechaInicio <= fechaFin && r.FechaFin >= fechaInicio)));

            if (excludeReservaId.HasValue)
            {
                query = query.Where(r => r.ReservaId != excludeReservaId.Value);
            }

            return !await query.AnyAsync();
        }

        public async Task<IEnumerable<Habitacion>> GetHabitacionesDisponiblesAsync(int hotelId, DateTime fechaInicio, DateTime fechaFin, int capacidad)
        {
            // Obtener todas las habitaciones del hotel que cumplan con la capacidad
            var habitaciones = await _context.Habitaciones
                .Include(h => h.Hotel)
                .Include(h => h.Imagenes)
                .Where(h => h.HotelId == hotelId && 
                           h.Capacidad >= capacidad && 
                           h.Disponible)
                .ToListAsync();

            // Filtrar las que estén disponibles en las fechas solicitadas
            var disponibles = new List<Habitacion>();
            foreach (var habitacion in habitaciones)
            {
                if (await IsHabitacionDisponibleAsync(habitacion.HabitacionId, fechaInicio, fechaFin))
                {
                    disponibles.Add(habitacion);
                }
            }

            return disponibles;
        }

        #endregion

        #region CRUD

        public async Task<Reserva> CreateAsync(ReservaCreateDto reservaDto)
        {
            // Validar fechas
            if (reservaDto.FechaInicio >= reservaDto.FechaFin)
            {
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio");
            }

            if (reservaDto.FechaInicio < DateTime.Today)
            {
                throw new InvalidOperationException("No se pueden hacer reservas en fechas pasadas");
            }

            // Verificar que la habitación existe
            var habitacion = await _context.Habitaciones.FindAsync(reservaDto.HabitacionId);
            if (habitacion == null)
            {
                throw new InvalidOperationException("La habitación especificada no existe");
            }

            // Verificar disponibilidad
            if (!await IsHabitacionDisponibleAsync(reservaDto.HabitacionId, reservaDto.FechaInicio, reservaDto.FechaFin))
            {
                throw new InvalidOperationException("La habitación no está disponible en las fechas seleccionadas");
            }

            // Calcular número de noches y precio total
            var numeroNoches = (reservaDto.FechaFin - reservaDto.FechaInicio).Days;
            var precioTotal = await CalcularPrecioTotalAsync(reservaDto.HabitacionId, reservaDto.FechaInicio, reservaDto.FechaFin);

            // Crear la reserva
          var reserva = new Reserva
{
    NumeroReserva = GenerarNumeroReserva(),
    UsuarioId = reservaDto.UsuarioId,
    HabitacionId = reservaDto.HabitacionId,
    HotelId = reservaDto.HotelId,
    FechaInicio = reservaDto.FechaInicio,
    FechaFin = reservaDto.FechaFin,
    NumeroHuespedes = reservaDto.NumeroHuespedes,
    GuestsAdults = reservaDto.GuestsAdults,
    GuestsChildren = reservaDto.GuestsChildren,
    GuestFirstName = reservaDto.GuestFirstName,
    GuestLastName = reservaDto.GuestLastName,
    GuestEmail = reservaDto.GuestEmail,
    GuestPhone = reservaDto.GuestPhone,
    NumeroNoches = numeroNoches,
    RoomRate = reservaDto.RoomRate,
    Subtotal = reservaDto.Subtotal,
    Taxes = reservaDto.Taxes,
    Currency = reservaDto.Currency,
    PrecioTotal = precioTotal,
    PaymentMethod = reservaDto.PaymentMethod,
    SolicitudesEspeciales = reservaDto.SolicitudesEspeciales,
    Estado = "pendiente",
    EstadoPago = "pendiente",
    FechaCreacion = DateTime.UtcNow
};

            _context.Reservas.Add(reserva);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reserva creada: {reserva.NumeroReserva}");

            return await GetByIdAsync(reserva.ReservaId) ?? reserva;
        }

        public async Task<Reserva?> UpdateAsync(int id, ReservaUpdateDto reservaDto)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return null;

            // Solo permitir actualizar si está en estado pendiente
            if (reserva.Estado != "pendiente")
            {
                throw new InvalidOperationException($"No se puede modificar una reserva en estado '{reserva.Estado}'");
            }

            // Validar fechas
            if (reservaDto.FechaInicio >= reservaDto.FechaFin)
            {
                throw new InvalidOperationException("La fecha de fin debe ser posterior a la fecha de inicio");
            }

            // Verificar disponibilidad si cambiaron las fechas
            if (reserva.FechaInicio != reservaDto.FechaInicio || reserva.FechaFin != reservaDto.FechaFin)
            {
                if (!await IsHabitacionDisponibleAsync(reserva.HabitacionId, reservaDto.FechaInicio, reservaDto.FechaFin, id))
                {
                    throw new InvalidOperationException("La habitación no está disponible en las nuevas fechas seleccionadas");
                }
            }

            // Actualizar datos
            reserva.FechaInicio = reservaDto.FechaInicio;
            reserva.FechaFin = reservaDto.FechaFin;
            reserva.NumeroHuespedes = reservaDto.NumeroHuespedes;
            reserva.NumeroNoches = (reservaDto.FechaFin - reservaDto.FechaInicio).Days;
            reserva.PrecioTotal = await CalcularPrecioTotalAsync(reserva.HabitacionId, reservaDto.FechaInicio, reservaDto.FechaFin);
            reserva.SolicitudesEspeciales = reservaDto.SolicitudesEspeciales;
            reserva.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return await GetByIdAsync(id);
        }

        public async Task<bool> CancelarReservaAsync(int id, string motivo)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return false;

            if (reserva.Estado == "cancelada")
            {
                throw new InvalidOperationException("La reserva ya está cancelada");
            }

            if (reserva.Estado == "completada")
            {
                throw new InvalidOperationException("No se puede cancelar una reserva completada");
            }

            reserva.Estado = "cancelada";
            reserva.EstadoPago = "reembolsado";
            reserva.SolicitudesEspeciales = (reserva.SolicitudesEspeciales ?? "") + $"\n[CANCELADA: {motivo}]";
            reserva.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reserva cancelada: {reserva.NumeroReserva}. Motivo: {motivo}");

            return true;
        }

        public async Task<bool> ConfirmarReservaAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return false;

            if (reserva.Estado != "pendiente")
            {
                throw new InvalidOperationException($"Solo se pueden confirmar reservas pendientes. Estado actual: {reserva.Estado}");
            }

            reserva.Estado = "confirmada";
            reserva.EstadoPago = "pagado";
            reserva.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reserva confirmada: {reserva.NumeroReserva}");

            return true;
        }

        public async Task<bool> CompletarReservaAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return false;

            if (reserva.Estado != "confirmada")
            {
                throw new InvalidOperationException($"Solo se pueden completar reservas confirmadas. Estado actual: {reserva.Estado}");
            }

            reserva.Estado = "completada";
            reserva.FechaModificacion = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reserva completada: {reserva.NumeroReserva}");

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var reserva = await _context.Reservas.FindAsync(id);
            if (reserva == null)
                return false;

            // Solo permitir eliminar reservas canceladas
            if (reserva.Estado != "cancelada")
            {
                throw new InvalidOperationException("Solo se pueden eliminar reservas canceladas. Cancela la reserva primero.");
            }

            _context.Reservas.Remove(reserva);
            await _context.SaveChangesAsync();

            _logger.LogInformation($"Reserva eliminada: {reserva.NumeroReserva}");

            return true;
        }

        #endregion

        #region Cálculos

        public async Task<decimal> CalcularPrecioTotalAsync(int habitacionId, DateTime fechaInicio, DateTime fechaFin)
        {
            var habitacion = await _context.Habitaciones.FindAsync(habitacionId);
            if (habitacion == null)
            {
                throw new InvalidOperationException("Habitación no encontrada");
            }

            var numeroNoches = (fechaFin - fechaInicio).Days;
            return habitacion.PrecioNoche * numeroNoches;
        }

        public string GenerarNumeroReserva()
        {
            // Formato: BK-YYYYMMDD-XXXX
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random();
            var numero = random.Next(1000, 9999);
            return $"BK-{fecha}-{numero}";
        }

        #endregion

        #region Estadísticas

        public async Task<int> GetTotalReservasActivasAsync()
        {
            return await _context.Reservas
                .Where(r => r.Estado == "confirmada" || r.Estado == "pendiente")
                .CountAsync();
        }

        public async Task<decimal> GetIngresosTotalesAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null)
        {
            var query = _context.Reservas
                .Where(r => r.EstadoPago == "pagado");

            if (fechaInicio.HasValue)
                query = query.Where(r => r.FechaCreacion >= fechaInicio.Value);

            if (fechaFin.HasValue)
                query = query.Where(r => r.FechaCreacion <= fechaFin.Value);

            return await query.SumAsync(r => r.PrecioTotal);
        }

        #endregion
    }
}