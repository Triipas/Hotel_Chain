// Data/AppDbContext.cs - Actualizado
using Microsoft.EntityFrameworkCore;
using Hotel_chain.Models.Entities;

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
        public DbSet<Huesped> Huespedes { get; set; }
        public DbSet<Staff> Staff { get; set; }
        public DbSet<Hotel> Hoteles { get; set; }
        public DbSet<Habitacion> Habitaciones { get; set; }
        public DbSet<Reserva> Reservas { get; set; }
        public DbSet<Imagen> Imagenes { get; set; }

        public DbSet<HotelAmenidad> HotelAmenidades { get; set; }
        public DbSet<HotelCaracteristica> HotelCaracteristicas { get; set; } = null!;
        public DbSet<HabitacionAmenidad> HabitacionAmenidades { get; set; } = null!;
        public DbSet<Reseña> Reseñas { get; set; } = null!;
        public DbSet<PermisoUsuario> PermisosUsuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Mapear tablas
            modelBuilder.Entity<Usuario>().ToTable("Usuarios");
            modelBuilder.Entity<Huesped>().ToTable("Huespedes");
            modelBuilder.Entity<Staff>().ToTable("Staff");
            modelBuilder.Entity<Hotel>().ToTable("Hoteles");
            modelBuilder.Entity<Habitacion>().ToTable("Habitaciones");
            modelBuilder.Entity<Reserva>().ToTable("Reservas");
            modelBuilder.Entity<Imagen>().ToTable("Imagenes");

            // ===== CONFIGURACIÓN DE USUARIOS =====
            modelBuilder.Entity<Usuario>().Property(u => u.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Usuario>().Property(u => u.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Usuario>().Property(u => u.Apellido).HasColumnName("apellido");
            modelBuilder.Entity<Usuario>().Property(u => u.Email).HasColumnName("email");
            modelBuilder.Entity<Usuario>().Property(u => u.Telefono).HasColumnName("telefono");
            modelBuilder.Entity<Usuario>().Property(u => u.Documento).HasColumnName("documento");
            modelBuilder.Entity<Usuario>().Property(u => u.Contraseña).HasColumnName("contraseña");
            modelBuilder.Entity<Usuario>().Property(u => u.Rol).HasColumnName("rol");
            modelBuilder.Entity<Usuario>().Property(u => u.Estado).HasColumnName("estado");
            modelBuilder.Entity<Usuario>().Property(u => u.FechaCreacion).HasColumnName("fecha_creacion");
            modelBuilder.Entity<Usuario>().Property(u => u.UltimoAcceso).HasColumnName("ultimo_acceso");

            // Índice único en Email
            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Email)
                .IsUnique();

            // ===== CONFIGURACIÓN DE HUÉSPEDES =====
            modelBuilder.Entity<Huesped>().Property(h => h.HuespedId).HasColumnName("huesped_id");
            modelBuilder.Entity<Huesped>().Property(h => h.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Huesped>().Property(h => h.Preferencias).HasColumnName("preferencias");
            modelBuilder.Entity<Huesped>().Property(h => h.NotasInternas).HasColumnName("notas_internas");

            // Relación Usuario-Huesped (1:1)
            modelBuilder.Entity<Huesped>()
                .HasOne(h => h.Usuario)
                .WithOne(u => u.Huesped)
                .HasForeignKey<Huesped>(h => h.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== CONFIGURACIÓN DE STAFF =====
            modelBuilder.Entity<Staff>().Property(s => s.StaffId).HasColumnName("staff_id");
            modelBuilder.Entity<Staff>().Property(s => s.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Staff>().Property(s => s.RolDetallado).HasColumnName("rol_detallado");
            modelBuilder.Entity<Staff>().Property(s => s.PermisosExtra).HasColumnName("permisos_extra");

            // Relación Usuario-Staff (1:1)
            modelBuilder.Entity<Staff>()
                .HasOne(s => s.Usuario)
                .WithOne(u => u.Staff)
                .HasForeignKey<Staff>(s => s.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);

            // ===== CONFIGURACIÓN DE HOTELES =====
            modelBuilder.Entity<Hotel>().Property(h => h.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Hotel>().Property(h => h.Nombre).HasColumnName("nombre");
            modelBuilder.Entity<Hotel>().Property(h => h.Direccion).HasColumnName("direccion");
            modelBuilder.Entity<Hotel>().Property(h => h.Ciudad).HasColumnName("ciudad");
            modelBuilder.Entity<Hotel>().Property(h => h.Descripcion).HasColumnName("descripcion");
            modelBuilder.Entity<Hotel>().Property(h => h.TelefonoContacto).HasColumnName("telefono_contacto");

            // ===== CONFIGURACIÓN DE HABITACIONES =====
            modelBuilder.Entity<Habitacion>().Property(h => h.HabitacionId).HasColumnName("habitacion_id");
            modelBuilder.Entity<Habitacion>().Property(h => h.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Habitacion>().Property(h => h.NumeroHabitacion).HasColumnName("numero_habitacion");
            modelBuilder.Entity<Habitacion>().Property(h => h.Tipo).HasColumnName("tipo");
            modelBuilder.Entity<Habitacion>().Property(h => h.Capacidad).HasColumnName("capacidad");
            modelBuilder.Entity<Habitacion>().Property(h => h.PrecioNoche).HasColumnName("precio_noche");
            modelBuilder.Entity<Habitacion>().Property(h => h.Descripcion).HasColumnName("descripcion");
            modelBuilder.Entity<Habitacion>().Property(h => h.Disponible).HasColumnName("disponible");

            // ===== CONFIGURACIÓN DE RESERVAS =====
            modelBuilder.Entity<Reserva>().Property(r => r.ReservaId).HasColumnName("reserva_id");
            modelBuilder.Entity<Reserva>().Property(r => r.NumeroReserva).HasColumnName("numero_reserva");
            modelBuilder.Entity<Reserva>().Property(r => r.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Reserva>().Property(r => r.HabitacionId).HasColumnName("habitacion_id");
            modelBuilder.Entity<Reserva>().Property(r => r.FechaInicio).HasColumnName("fecha_inicio");
            modelBuilder.Entity<Reserva>().Property(r => r.FechaFin).HasColumnName("fecha_fin");
            modelBuilder.Entity<Reserva>().Property(r => r.NumeroHuespedes).HasColumnName("numero_huespedes");
            modelBuilder.Entity<Reserva>().Property(r => r.NumeroNoches).HasColumnName("numero_noches");
            modelBuilder.Entity<Reserva>().Property(r => r.PrecioTotal).HasColumnName("precio_total");
            modelBuilder.Entity<Reserva>().Property(r => r.Estado).HasColumnName("estado");
            modelBuilder.Entity<Reserva>().Property(r => r.EstadoPago).HasColumnName("estado_pago");
            modelBuilder.Entity<Reserva>().Property(r => r.SolicitudesEspeciales).HasColumnName("solicitudes_especiales");
            modelBuilder.Entity<Reserva>().Property(r => r.FechaCreacion).HasColumnName("fecha_creacion");
            modelBuilder.Entity<Reserva>().Property(r => r.FechaModificacion).HasColumnName("fecha_modificacion");

            // Índice único en NumeroReserva
            modelBuilder.Entity<Reserva>()
                .HasIndex(r => r.NumeroReserva)
                .IsUnique();

            // ===== CONFIGURACIÓN DE IMÁGENES =====
            modelBuilder.Entity<Imagen>().Property(i => i.Id).HasColumnName("Id");
            modelBuilder.Entity<Imagen>().Property(i => i.NombreArchivo).HasColumnName("NombreArchivo");
            modelBuilder.Entity<Imagen>().Property(i => i.HotelId).HasColumnName("HotelId");
            modelBuilder.Entity<Imagen>().Property(i => i.HabitacionId).HasColumnName("HabitacionId");

            // ===== CONFIGURAR TIPOS DE DATOS =====
            modelBuilder.Entity<Habitacion>()
                .Property(h => h.PrecioNoche)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<Reserva>()
                .Property(r => r.PrecioTotal)
                .HasColumnType("decimal(10,2)");
        }
    }
}