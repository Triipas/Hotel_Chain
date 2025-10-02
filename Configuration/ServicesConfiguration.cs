// Configuration/ServicesConfiguration.cs
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Services.Implementation;
using Hotel_chain.Filters;

namespace Hotel_chain.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services)
        {
            // Registrar servicios de lógica de negocio
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IHabitacionService, HabitacionService>();
            services.AddScoped<IAdminAuthService, AdminAuthService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            
            // TODO: Agregar otros servicios cuando los creemos
            // services.AddScoped<IReservaService, ReservaService>();

            // Registrar filtros
            services.AddScoped<AdminAuthFilter>();

            return services;
        }
    }
}