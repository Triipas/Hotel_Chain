// Services/Interfaces/IReservaService.cs
using Hotel_chain.Models.Entities;
using Hotel_chain.Models.DTOs.Reserva;

namespace Hotel_chain.Services.Interfaces
{
    public interface IReservaService
    {
        // Consultas
        Task<IEnumerable<Reserva>> GetAllAsync();
        Task<Reserva?> GetByIdAsync(int id);
        Task<Reserva?> GetByNumeroReservaAsync(string numeroReserva);
        Task<IEnumerable<Reserva>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Reserva>> GetByHabitacionIdAsync(int habitacionId);
        Task<IEnumerable<Reserva>> SearchAsync(string? estado, string? estadoPago, DateTime? fechaInicio, DateTime? fechaFin);
        
        // Verificación de disponibilidad
        Task<bool> IsHabitacionDisponibleAsync(int habitacionId, DateTime fechaInicio, DateTime fechaFin, int? excludeReservaId = null);
        Task<IEnumerable<Habitacion>> GetHabitacionesDisponiblesAsync(int hotelId, DateTime fechaInicio, DateTime fechaFin, int capacidad);
        
        // CRUD
        Task<Reserva> CreateAsync(ReservaCreateDto reservaDto);
        Task<Reserva?> UpdateAsync(int id, ReservaUpdateDto reservaDto);
        Task<bool> CancelarReservaAsync(int id, string motivo);
        Task<bool> ConfirmarReservaAsync(int id);
        Task<bool> CompletarReservaAsync(int id);
        Task<bool> DeleteAsync(int id);
        
        // Cálculos
        Task<decimal> CalcularPrecioTotalAsync(int habitacionId, DateTime fechaInicio, DateTime fechaFin);
        string GenerarNumeroReserva();
        
        // Estadísticas
        Task<int> GetTotalReservasActivasAsync();
        Task<decimal> GetIngresosTotalesAsync(DateTime? fechaInicio = null, DateTime? fechaFin = null);
    }
}