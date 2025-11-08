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
        private readonly IHotelAmenidadService _hotelAmenidadService;
        private readonly IHabitacionAmenidadService _habitacionAmenidadService;

        public HotelPlugin(
            IHotelService hotelService,
            IHabitacionService habitacionService,
            IReservaService reservaService,
            IHotelAmenidadService hotelAmenidadService,
            IHabitacionAmenidadService habitacionAmenidadService)
        {
            _hotelService = hotelService;
            _habitacionService = habitacionService;
            _reservaService = reservaService;
            _hotelAmenidadService = hotelAmenidadService;
            _habitacionAmenidadService = habitacionAmenidadService;
        }

        [KernelFunction, Description("Busca hoteles por ciudad, nombre o amenidades específicas")]
        public async Task<string> BuscarHoteles(
            [Description("La ciudad donde buscar hoteles, por ejemplo: Lima, Cusco, Arequipa")] string? ciudad = null,
            [Description("Nombre del hotel a buscar")] string? nombre = null,
            [Description("Amenidades requeridas (ej: piscina, wifi, spa)")] string? amenidades = null)
        {
            try
            {
                var hoteles = await _hotelService.SearchAsync(ciudad, nombre);
                
                // Filtrar por amenidades si se especificaron
                if (!string.IsNullOrEmpty(amenidades) && hoteles.Any())
                {
                    var amenidadesRequeridas = amenidades.ToLower()
                        .Split(',')
                        .Select(a => a.Trim())
                        .ToList();
                    
                    var hotelesFiltrados = new List<Hotel_chain.Models.Entities.Hotel>();
                    foreach (var hotel in hoteles)
                    {
                        var amenidadesHotel = await _hotelAmenidadService.GetByHotelIdAsync(hotel.HotelId);
                        var tieneAmenidades = amenidadesRequeridas.All(ar => 
                            amenidadesHotel.Any(ah => ah.Amenidad.ToLower().Contains(ar)));
                        
                        if (tieneAmenidades)
                        {
                            hotelesFiltrados.Add(hotel);
                        }
                    }
                    hoteles = hotelesFiltrados;
                }
                
                if (!hoteles.Any())
                {
                    return amenidades != null 
                        ? "No se encontraron hoteles con esas amenidades y criterios."
                        : "No se encontraron hoteles con esos criterios.";
                }

                var resultado = $"Hoteles encontrados ({hoteles.Count()}):\n\n";
                foreach (var hotel in hoteles.Take(5))
                {
                    resultado += $"🏨 {hotel.Nombre}\n";
                    resultado += $"📍 {hotel.Ciudad}, {hotel.Direccion}\n";
                    resultado += $"📞 {hotel.TelefonoContacto ?? "No disponible"}\n";
                    
                    if (hotel.Calificacion.HasValue)
                    {
                        resultado += $"⭐ Calificación: {hotel.Calificacion:F1}/5\n";
                    }
                    
                    // Mostrar amenidades destacadas
                    var amenidadesHotel = await _hotelAmenidadService.GetByHotelIdAsync(hotel.HotelId);
                    if (amenidadesHotel.Any())
                    {
                        resultado += $"✨ Amenidades: {string.Join(", ", amenidadesHotel.Take(3).Select(a => a.Amenidad))}";
                        if (amenidadesHotel.Count() > 3)
                        {
                            resultado += $" y {amenidadesHotel.Count() - 3} más";
                        }
                        resultado += "\n";
                    }
                    resultado += $"🆔 ID: {hotel.HotelId}\n\n";
                }

                if (hoteles.Count() > 5)
                {
                    resultado += $"... y {hoteles.Count() - 5} hoteles más. Usa un filtro más específico para ver otros resultados.\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al buscar hoteles: {ex.Message}";
            }
        }

        [KernelFunction, Description("Busca habitaciones disponibles en un hotel específico con sus amenidades")]
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

                var habitacionesDisponibles = habitaciones.Where(h => h.Disponible).ToList();
                
                if (!habitacionesDisponibles.Any())
                {
                    return "No hay habitaciones disponibles en este momento para esos criterios.";
                }

                var resultado = $"Habitaciones disponibles ({habitacionesDisponibles.Count}):\n\n";
                
                foreach (var hab in habitacionesDisponibles.Take(5))
                {
                    resultado += $"🛏️ Habitación {hab.NumeroHabitacion} - {hab.Tipo}\n";
                    resultado += $"👥 Capacidad: {hab.Capacidad} persona(s)\n";
                    resultado += $"💰 Precio por noche: S/ {hab.PrecioNoche:F2}\n";
                    
                    if (!string.IsNullOrEmpty(hab.Descripcion))
                    {
                        resultado += $"📝 {hab.Descripcion}\n";
                    }
                    
                    // Mostrar amenidades de la habitación
                    var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(hab.HabitacionId);
                    if (amenidades.Any())
                    {
                        resultado += $"✨ Incluye: {string.Join(", ", amenidades.Select(a => a.Amenidad))}\n";
                    }
                    
                    resultado += $"🆔 ID Habitación: {hab.HabitacionId}\n\n";
                }

                if (habitacionesDisponibles.Count > 5)
                {
                    resultado += $"... y {habitacionesDisponibles.Count - 5} habitaciones más.\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al buscar habitaciones: {ex.Message}";
            }
        }

        [KernelFunction, Description("Lista todas las amenidades disponibles de un hotel específico")]
        public async Task<string> ListarAmenidadesHotel(
            [Description("ID del hotel")] int hotelId)
        {
            try
            {
                var hotel = await _hotelService.GetByIdAsync(hotelId);
                if (hotel == null)
                {
                    return "No se encontró el hotel especificado.";
                }

                var amenidades = await _hotelAmenidadService.GetByHotelIdAsync(hotelId);
                
                if (!amenidades.Any())
                {
                    return $"El hotel {hotel.Nombre} no tiene amenidades registradas.";
                }

                var resultado = $"✨ Amenidades de {hotel.Nombre}:\n\n";
                
                foreach (var amenidad in amenidades)
                {
                    resultado += $"  • {amenidad.Amenidad}\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al listar amenidades: {ex.Message}";
            }
        }

        [KernelFunction, Description("Busca hoteles que tengan amenidades específicas")]
        public async Task<string> BuscarHotelesPorAmenidades(
            [Description("Amenidades requeridas separadas por comas (ej: piscina, wifi, gimnasio)")] string amenidades)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(amenidades))
                {
                    return "Por favor especifica al menos una amenidad para buscar.";
                }

                var amenidadesRequeridas = amenidades.ToLower()
                    .Split(',')
                    .Select(a => a.Trim())
                    .ToList();

                var todosHoteles = await _hotelService.GetAllAsync();
                var hotelesConAmenidades = new List<(Hotel_chain.Models.Entities.Hotel Hotel, int Coincidencias)>();

                foreach (var hotel in todosHoteles)
                {
                    var amenidadesHotel = await _hotelAmenidadService.GetByHotelIdAsync(hotel.HotelId);
                    var coincidencias = amenidadesRequeridas.Count(ar => 
                        amenidadesHotel.Any(ah => ah.Amenidad.ToLower().Contains(ar)));
                    
                    if (coincidencias > 0)
                    {
                        hotelesConAmenidades.Add((hotel, coincidencias));
                    }
                }

                if (!hotelesConAmenidades.Any())
                {
                    return $"No se encontraron hoteles con las amenidades: {amenidades}";
                }

                // Ordenar por número de coincidencias
                hotelesConAmenidades = hotelesConAmenidades
                    .OrderByDescending(h => h.Coincidencias)
                    .ToList();

                var resultado = $"Hoteles con amenidades solicitadas:\n\n";
                
                foreach (var (hotel, coincidencias) in hotelesConAmenidades.Take(5))
                {
                    resultado += $"🏨 {hotel.Nombre} - {hotel.Ciudad}\n";
                    resultado += $"✅ {coincidencias}/{amenidadesRequeridas.Count} amenidades encontradas\n";
                    
                    var amenidadesHotel = await _hotelAmenidadService.GetByHotelIdAsync(hotel.HotelId);
                    resultado += $"✨ {string.Join(", ", amenidadesHotel.Take(4).Select(a => a.Amenidad))}\n";
                    resultado += $"🆔 ID: {hotel.HotelId}\n\n";
                }

                return resultado;
            }
            catch (Exception ex)
            {
                return $"Error al buscar hoteles por amenidades: {ex.Message}";
            }
        }

        [KernelFunction, Description("Obtiene información detallada de un hotel por su ID, incluyendo amenidades")]
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
                
                resultado += $"\n🛏️ Total de habitaciones: {hotel.Habitaciones?.Count ?? 0}\n";
                
                if (hotel.MascotasPermitidas.HasValue)
                {
                    resultado += $"🐕 Mascotas: {(hotel.MascotasPermitidas.Value ? "Permitidas" : "No permitidas")}\n";
                }
                
                if (hotel.FumarPermitido.HasValue)
                {
                    resultado += $"🚭 Fumar: {(hotel.FumarPermitido.Value ? "Permitido" : "No permitido")}\n";
                }

                // Agregar amenidades
                var amenidades = await _hotelAmenidadService.GetByHotelIdAsync(hotelId);
                if (amenidades.Any())
                {
                    resultado += $"\n✨ Amenidades ({amenidades.Count()}):\n";
                    foreach (var amenidad in amenidades.Take(10))
                    {
                        resultado += $"  • {amenidad.Amenidad}\n";
                    }
                    if (amenidades.Count() > 10)
                    {
                        resultado += $"  ... y {amenidades.Count() - 10} más\n";
                    }
                }
                
                if (!string.IsNullOrEmpty(hotel.Descripcion))
                {
                    resultado += $"\n📝 Descripción:\n{hotel.Descripcion}\n";
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

                var resultado = $"📋 Tenemos {hoteles.Count()} hotel(es) disponible(s):\n\n";
                
                foreach (var hotel in hoteles.Take(10))
                {
                    resultado += $"• {hotel.Nombre} - {hotel.Ciudad}";
                    if (hotel.Calificacion.HasValue)
                    {
                        resultado += $" ⭐{hotel.Calificacion:F1}";
                    }
                    resultado += $" (ID: {hotel.HotelId})\n";
                }

                if (hoteles.Count() > 10)
                {
                    resultado += $"\n... y {hoteles.Count() - 10} más. Usa filtros para ver hoteles específicos.\n";
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
                var resultado = $"✅ ¡Buenas noticias! Encontré {habitacionesDisponibles.Count()} habitación(es) disponible(s):\n\n";
                resultado += $"📅 Del {inicio:dd/MM/yyyy} al {fin:dd/MM/yyyy} ({noches} noche(s))\n";
                resultado += $"👥 Para {numeroHuespedes} persona(s)\n\n";

                foreach (var hab in habitacionesDisponibles.Take(5))
                {
                    var precioTotal = hab.PrecioNoche * noches;
                    resultado += $"🛏️ Habitación {hab.NumeroHabitacion} - {hab.Tipo}\n";
                    resultado += $"👥 Capacidad: {hab.Capacidad} personas\n";
                    resultado += $"💰 S/ {hab.PrecioNoche:F2} por noche\n";
                    resultado += $"💵 Total: S/ {precioTotal:F2}\n";
                    
                    // Mostrar amenidades
                    var amenidades = await _habitacionAmenidadService.GetByHabitacionIdAsync(hab.HabitacionId);
                    if (amenidades.Any())
                    {
                        resultado += $"✨ {string.Join(", ", amenidades.Take(3).Select(a => a.Amenidad))}\n";
                    }
                    resultado += $"🆔 ID: {hab.HabitacionId}\n\n";
                }

                if (habitacionesDisponibles.Count() > 5)
                {
                    resultado += $"... y {habitacionesDisponibles.Count() - 5} opciones más.\n";
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
            return @"¡Hola! 👋 Soy tu asistente virtual de Costa Dorada. 

Puedo ayudarte con:

🏨 HOTELES
  • Buscar hoteles por ciudad o nombre
  • Ver información detallada de hoteles
  • Listar todos los hoteles disponibles

🛏️ HABITACIONES
  • Buscar habitaciones disponibles
  • Ver tipos de habitaciones y precios
  • Consultar amenidades incluidas

📅 RESERVAS
  • Verificar disponibilidad para fechas específicas
  • Calcular precios totales
  • Consultar capacidad de habitaciones

✨ AMENIDADES
  • Ver amenidades de hoteles y habitaciones
  • Buscar hoteles por amenidades específicas
  • Consultar servicios incluidos

💡 EJEMPLOS DE PREGUNTAS:
  • ""¿Qué hoteles tienen en Lima?""
  • ""Muéstrame hoteles con piscina y spa""
  • ""¿Hay habitaciones disponibles del 15 al 20 de enero para 2 personas?""
  • ""¿Qué amenidades tiene el hotel X?""
  • ""Quiero una habitación doble en Cusco""

¿En qué puedo ayudarte hoy? 😊";
        }
    }
}