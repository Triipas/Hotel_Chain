using Microsoft.EntityFrameworkCore;
using Hotel_chain.Models;

namespace Hotel_chain.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        // Tablas
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Rol> Roles { get; set; }
        public DbSet<Hotel> Hoteles { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapear tablas
            modelBuilder.Entity<Hotel>().ToTable("Hoteles");
            modelBuilder.Entity<Habitacion>().ToTable("Habitaciones");
            modelBuilder.Entity<Reserva>().ToTable("Reservas");
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Imagen>().ToTable("Imagenes");
            modelBuilder.Entity<Rol>().ToTable("Roles");

            // ===== MAPEAR COLUMNAS DE HOTELES =====
            modelBuilder.Entity<Hotel>().Property(h => h.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Hotel>().Property(h => h.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Hotel>().Property(h => h.Direccion).HasColumnName("direccion");
            modelBuilder.Entity<Hotel>().Property(h => h.Ciudad).HasColumnName("ciudad");
            modelBuilder.Entity<Hotel>().Property(h => h.Descripcion).HasColumnName("descripcion");
            modelBuilder.Entity<Hotel>().Property(h => h.TelefonoContacto).HasColumnName("telefono_contacto");

            // ===== MAPEAR COLUMNAS DE HABITACIONES =====
            modelBuilder.Entity<Habitacion>().Property(h => h.HabitacionId).HasColumnName("habitacion_id");
            modelBuilder.Entity<Habitacion>().Property(h => h.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Habitacion>().Property(h => h.NumeroHabitacion).HasColumnName("numero_habitacion");
            modelBuilder.Entity<Habitacion>().Property(h => h.Tipo).HasColumnName("tipo");
            modelBuilder.Entity<Habitacion>().Property(h => h.Capacidad).HasColumnName("capacidad");
            modelBuilder.Entity<Habitacion>().Property(h => h.PrecioNoche).HasColumnName("precio_noche");
            modelBuilder.Entity<Habitacion>().Property(h => h.Descripcion).HasColumnName("descripcion");
            modelBuilder.Entity<Habitacion>().Property(h => h.Disponible).HasColumnName("disponible");

            // ===== MAPEAR COLUMNAS DE RESERVAS =====
            modelBuilder.Entity<Reserva>().Property(r => r.ReservaId).HasColumnName("reserva_id");
            modelBuilder.Entity<Reserva>().Property(r => r.HabitacionId).HasColumnName("habitacion_id");
            modelBuilder.Entity<Reserva>().Property(r => r.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Reserva>().Property(r => r.FechaInicio).HasColumnName("fecha_inicio"); // 🆕 MAPEO FALTANTE
            modelBuilder.Entity<Reserva>().Property(r => r.FechaFin).HasColumnName("fecha_fin"); // 🆕 MAPEO FALTANTE
            modelBuilder.Entity<Reserva>().Property(r => r.PrecioTotal).HasColumnName("precio_total"); // 🆕 MAPEO FALTANTE
            modelBuilder.Entity<Reserva>().Property(r => r.Estado).HasColumnName("estado"); // 🆕 MAPEO FALTANTE

            // ===== MAPEAR COLUMNAS DE USUARIOS =====
            modelBuilder.Entity<Usuario>().Property(u => u.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Usuario>().Property(u => u.Apellido).HasColumnName("apellido");
            modelBuilder.Entity<Usuario>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<Usuario>().Property(u => u.Contra).HasColumnName("contra");
            modelBuilder.Entity<Usuario>().Property(u => u.Telefono).HasColumnName("telefono");

            // ===== MAPEAR COLUMNAS DE ROLES =====
            modelBuilder.Entity<Rol>().Property(r => r.RolId).HasColumnName("rol_id");
            modelBuilder.Entity<Rol>().Property(r => r.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Rol>().Property(r => r.Apellido).HasColumnName("apellido");
            modelBuilder.Entity<Rol>().Property(r => r.Email).HasColumnName("email");
            modelBuilder.Entity<Rol>().Property(r => r.Contraseña).HasColumnName("contraseña");
            modelBuilder.Entity<Rol>().Property(r => r.Telefono).HasColumnName("telefono");
            modelBuilder.Entity<Rol>().Property(r => r.Puesto).HasColumnName("puesto");

            // ===== MAPEAR COLUMNAS DE IMAGENES =====
            modelBuilder.Entity<Imagen>().Property(i => i.Id).HasColumnName("Id");
            modelBuilder.Entity<Imagen>().Property(i => i.NombreArchivo).HasColumnName("NombreArchivo");
            modelBuilder.Entity<Imagen>().Property(i => i.HotelId).HasColumnName("HotelId");
            modelBuilder.Entity<Imagen>().Property(i => i.HabitacionId).HasColumnName("HabitacionId");

            // ===== CONFIGURAR TIPOS DE DATOS =====
            // Guardar enums como string
            modelBuilder.Entity<Habitacion>()
                .Property(h => h.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Estado)
                .HasConversion<string>();

            // Configurar decimales
            modelBuilder.Entity<Habitacion>()
                .Property(h => h.PrecioNoche)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Reserva>()
                .Property(r => r.PrecioTotal)
                .HasColumnType("decimal(10,2)");
        }
    }
}