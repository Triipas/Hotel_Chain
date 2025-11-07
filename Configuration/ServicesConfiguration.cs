
using Hotel_chain.Services.Interfaces;
using Hotel_chain.Services.Implementation;
using Hotel_chain.Filters;

namespace Hotel_chain.Configuration
{
    public static class ServicesConfiguration
    {
        public static IServiceCollection ConfigureBusinessServices(this IServiceCollection services)
        {
         
            services.AddScoped<IHotelService, HotelService>();
            services.AddScoped<IHabitacionService, HabitacionService>();
            services.AddScoped<IAdminAuthService, AdminAuthService>();
            services.AddScoped<IUsuarioService, UsuarioService>();
            services.AddScoped<IResenaService, ResenaService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IHabitacionAmenidadService, HabitacionAmenidadService>();
            services.AddScoped<IHotelAmenidadService, HotelAmenidadService>();
            services.AddScoped<IHotelCaracteristicaService, HotelCaracteristicaService>();
            services.AddScoped<IChatService, ChatService>();
            services.AddScoped<AdminAuthFilter>();

            return services;
        }
    }
}