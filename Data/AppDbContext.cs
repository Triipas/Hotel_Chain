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

            // Mapear columnas
            modelBuilder.Entity<Hotel>().Property(h => h.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Habitacion>().Property(h => h.HabitacionId).HasColumnName("habitacion_id");
            modelBuilder.Entity<Habitacion>().Property(h => h.HotelId).HasColumnName("hotel_id");
            modelBuilder.Entity<Reserva>().Property(r => r.ReservaId).HasColumnName("reserva_id");
            modelBuilder.Entity<Reserva>().Property(r => r.HabitacionId).HasColumnName("habitacion_id");
            modelBuilder.Entity<Reserva>().Property(r => r.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Usuario>().Property(u => u.UsuarioId).HasColumnName("usuario_id");
            modelBuilder.Entity<Imagen>().Property(i => i.Id).HasColumnName("Id");
            modelBuilder.Entity<Imagen>().Property(i => i.HotelId).HasColumnName("HotelId");
            modelBuilder.Entity<Imagen>().Property(i => i.HabitacionId).HasColumnName("HabitacionId");
            modelBuilder.Entity<Rol>().Property(r => r.RolId).HasColumnName("rol_id");
            modelBuilder.Entity<Hotel>().Property(h => h.TelefonoContacto).HasColumnName("telefono_contacto");

            // Guardar enums como string
            modelBuilder.Entity<Habitacion>()
                .Property(h => h.Tipo)
                .HasConversion<string>();

            modelBuilder.Entity<Reserva>()
                .Property(r => r.Estado)
                .HasConversion<string>();
        }
    }
}