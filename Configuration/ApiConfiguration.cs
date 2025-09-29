// Configuration/ApiConfiguration.cs
namespace Hotel_chain.Configuration
{
    public static class ApiConfiguration
    {
        public static IServiceCollection ConfigureApiServices(this IServiceCollection services)
        {
            // Servicios API para los controladores API
            services.AddControllers()
                .AddJsonOptions(options =>
                {
                    // Configurar JSON para la API
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Mantener PascalCase
                    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
                });

            // CORS para permitir que el frontend admin consuma la API
            services.AddCors(options =>
            {
                options.AddPolicy("AdminPanel", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                });
            });

            return services;
        }
    }
}
