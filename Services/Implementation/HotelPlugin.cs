using System.ComponentModel;
using Microsoft.SemanticKernel;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class HotelPlugin
    {
        private readonly IHotelService _hotelService;
        private readonly IHabitacionService _habitacionService;
        private readonly IReservaService _reservaService;

        public HotelPlugin(
            IHotelService hotelService,
            IHabitacionService habitacionService,
            IReservaService reservaService)
        {
            _hotelService = hotelService;
            _habitacionService = habitacionService;
            _reservaService = reservaService;
        }

        [KernelFunction, Description("Busca hoteles por ciudad o nombre")]
        public async Task<string> BuscarHoteles(
            [Description("La ciudad donde buscar hoteles, por ejemplo: Lima, Cusco, Arequipa")] string? ciudad = null,
            [Description("Nombre del hotel a buscar")] string? nombre = null)
        {
            try
            {
                var hoteles = await _hotelService.SearchAsync(ciudad, nombre);
                
                if (!hoteles.Any())
                {
                    return "No se encontraron hoteles con esos criterios.";
                }

                var resultado = "Hoteles encontrados:\n\n";
                foreach (var hotel in hoteles.Take(5)) // Limitar a 5 resultados
                {
                    resultado += $"🏨 {hotel.Nombre}\n";
                    resultado += $"📍 {hotel.Ciudad}, {hotel.Direccion}\n";
                    resultado += $"📞 {hotel.TelefonoContacto ?? "No disponible"}\n";
                    if (hotel.Calificacion.HasValue)
                    {
                        resultado += $"⭐ Calificación: {hotel.Calificacion:F1}/5\n";
                    }
                    resultado += "\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al buscar hoteles: {ex.Message}";
            }
        }

        [KernelFunction, Description("Busca habitaciones disponibles en un hotel específico")]
        public async Task<string> BuscarHabitaciones(
            [Description("ID del hotel donde buscar habitaciones")] int hotelId,
            [Description("Tipo de habitación: simple, doble o suite")] string? tipo = null,
            [Description("Capacidad mínima de personas")] int? capacidadMinima = null)
        {
            try
            {
                var habitaciones = await _habitacionService.SearchAsync(hotelId, tipo, capacidadMinima);
                
                if (!habitaciones.Any())
                {
                    return "No se encontraron habitaciones disponibles con esos criterios.";
                }

                var resultado = "Habitaciones disponibles:\n\n";
                foreach (var hab in habitaciones.Where(h => h.Disponible).Take(5))
                {
                    resultado += $"🛏️ Habitación {hab.NumeroHabitacion} - {hab.Tipo}\n";
                    resultado += $"👥 Capacidad: {hab.Capacidad} personas\n";
                    resultado += $"💰 Precio por noche: S/ {hab.PrecioNoche:F2}\n";
                    if (!string.IsNullOrEmpty(hab.Descripcion))
                    {
                        resultado += $"📝 {hab.Descripcion}\n";
                    }
                    resultado += "\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al buscar habitaciones: {ex.Message}";
            }
        }

        [KernelFunction, Description("Obtiene información detallada de un hotel por su ID")]
        public async Task<string> ObtenerDetallesHotel(
            [Description("ID del hotel")] int hotelId)
        {
            try
            {
                var hotel = await _hotelService.GetByIdAsync(hotelId);
                
                if (hotel == null)
                {
                    return "No se encontró el hotel con ese ID.";
                }

                var resultado = $"🏨 {hotel.Nombre}\n\n";
                resultado += $"📍 Ubicación: {hotel.Direccion}, {hotel.Ciudad}";
                
                if (!string.IsNullOrEmpty(hotel.Pais))
                {
                    resultado += $", {hotel.Pais}";
                }
                
                resultado += "\n";
                
                if (!string.IsNullOrEmpty(hotel.TelefonoContacto))
                {
                    resultado += $"📞 Teléfono: {hotel.TelefonoContacto}\n";
                }
                
                if (hotel.Calificacion.HasValue)
                {
                    resultado += $"⭐ Calificación: {hotel.Calificacion:F1}/5\n";
                }
                
                if (!string.IsNullOrEmpty(hotel.Descripcion))
                {
                    resultado += $"\n📝 Descripción:\n{hotel.Descripcion}\n";
                }
                
                resultado += $"\n🛏️ Total de habitaciones: {hotel.Habitaciones?.Count ?? 0}\n";
                
                if (hotel.MascotasPermitidas.HasValue)
                {
                    resultado += $"🐕 Mascotas: {(hotel.MascotasPermitidas.Value ? "Permitidas" : "No permitidas")}\n";
                }
                
                if (hotel.FumarPermitido.HasValue)
                {
                    resultado += $"🚭 Fumar: {(hotel.FumarPermitido.Value ? "Permitido" : "No permitido")}\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al obtener detalles del hotel: {ex.Message}";
            }
        }

        [KernelFunction, Description("Lista todos los hoteles disponibles en el sistema")]
        public async Task<string> ListarTodosLosHoteles()
        {
            try
            {
                var hoteles = await _hotelService.GetAllAsync();
                
                if (!hoteles.Any())
                {
                    return "No hay hoteles registrados en el sistema.";
                }

                var resultado = $"Tenemos {hoteles.Count()} hoteles disponibles:\n\n";
                
                foreach (var hotel in hoteles.Take(10)) // Limitar a 10
                {
                    resultado += $"• {hotel.Nombre} - {hotel.Ciudad}\n";
                }

                if (hoteles.Count() > 10)
                {
                    resultado += $"\n... y {hoteles.Count() - 10} más.\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al listar hoteles: {ex.Message}";
            }
        }

        [KernelFunction, Description("Verifica disponibilidad de habitaciones para fechas específicas")]
        public async Task<string> VerificarDisponibilidad(
            [Description("ID del hotel")] int hotelId,
            [Description("Fecha de inicio en formato YYYY-MM-DD")] string fechaInicio,
            [Description("Fecha de fin en formato YYYY-MM-DD")] string fechaFin,
            [Description("Número de huéspedes")] int numeroHuespedes)
        {
            try
            {
                if (!DateTime.TryParse(fechaInicio, out var inicio) ||
                    !DateTime.TryParse(fechaFin, out var fin))
                {
                    return "Por favor proporciona fechas válidas en formato YYYY-MM-DD (ej: 2025-01-15).";
                }

                if (inicio < DateTime.Today)
                {
                    return "La fecha de inicio no puede ser anterior a hoy.";
                }

                if (fin <= inicio)
                {
                    return "La fecha de fin debe ser posterior a la fecha de inicio.";
                }

                var habitacionesDisponibles = await _reservaService.GetHabitacionesDisponiblesAsync(
                    hotelId, inicio, fin, numeroHuespedes);

                if (!habitacionesDisponibles.Any())
                {
                    return $"Lo siento, no hay habitaciones disponibles para {numeroHuespedes} persona(s) entre {inicio:dd/MM/yyyy} y {fin:dd/MM/yyyy}.";
                }

                var noches = (fin - inicio).Days;
                var resultado = $"¡Buenas noticias! Encontré {habitacionesDisponibles.Count()} habitación(es) disponible(s):\n\n";

                foreach (var hab in habitacionesDisponibles.Take(5))
                {
                    var precioTotal = hab.PrecioNoche * noches;
                    resultado += $"🛏️ Habitación {hab.NumeroHabitacion} - {hab.Tipo}\n";
                    resultado += $"👥 Capacidad: {hab.Capacidad} personas\n";
                    resultado += $"💰 S/ {hab.PrecioNoche:F2} por noche\n";
                    resultado += $"💵 Total por {noches} noche(s): S/ {precioTotal:F2}\n\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al verificar disponibilidad: {ex.Message}";
            }
        }

        [KernelFunction, Description("Proporciona ayuda sobre cómo usar el sistema de reservas")]
        public string ObtenerAyuda()
        {
            return @"¡Hola! Soy tu asistente virtual de Costa Dorada. Puedo ayudarte con:

🏨 Buscar hoteles por ciudad o nombre
🛏️ Ver habitaciones disponibles
📅 Verificar disponibilidad para fechas específicas
💰 Consultar precios
ℹ️ Información detallada de hoteles

Ejemplos de preguntas:
- ""¿Qué hoteles tienen en Lima?""
- ""Muéstrame habitaciones en el hotel X""
- ""¿Hay habitaciones disponibles del 15 al 20 de enero?""
- ""¿Cuánto cuesta una habitación doble?""

¿En qué puedo ayudarte hoy?";
        }
    }
}