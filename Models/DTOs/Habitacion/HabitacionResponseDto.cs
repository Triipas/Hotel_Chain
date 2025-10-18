namespace Hotel_chain.Models.DTOs.Habitacion
{
    public class HabitacionResponseDto
    {
        public int HabitacionId { get; set; }
        public int HotelId { get; set; }
        public string HotelNombre { get; set; } = null!;
        public string NumeroHabitacion { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public string? Nombre { get; set; }
        public int Capacidad { get; set; }
        public int? CapacidadAdultos { get; set; }
        public int? CapacidadNinos { get; set; }
        public int? CantidadCamas { get; set; }
        public string? TipoCama { get; set; }
        public int? TamanoM2 { get; set; }

        public decimal PrecioNoche { get; set; }
        public decimal? PrecioBase { get; set; }
        public string? Descripcion { get; set; }
        public bool Disponible { get; set; }
        public List<string> Imagenes { get; set; } = new();
    }
}