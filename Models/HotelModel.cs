using System;
using System.Collections.Generic;

namespace Hotel_chain.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string Rol { get; set; } = "Cliente";
    }

    public class Hotel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Direccion { get; set; } = null!;
        public string Ciudad { get; set; } = null!;
        public string Telefono { get; set; } = null!;
        public string Descripcion { get; set; } = null!;
        public ICollection<Habitacion> Habitaciones { get; set; } = new List<Habitacion>();
    }

    public class Habitacion
    {
        public int Id { get; set; }
        public string Numero { get; set; } = null!;
        public string Tipo { get; set; } = null!;
        public decimal Precio { get; set; }
        public bool Disponible { get; set; } = true;

        public int HotelId { get; set; }
        public Hotel Hotel { get; set; } = null!;
    }

    public class Reserva
    {
        public int Id { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public int UsuarioId { get; set; }
        public Usuario Usuario { get; set; } = null!;

        public int HabitacionId { get; set; }
        public Habitacion Habitacion { get; set; } = null!;
    }

}