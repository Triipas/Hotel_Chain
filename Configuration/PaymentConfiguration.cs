using MercadoPago.Config;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Hotel_chain.Configuration
{
    public static class PaymentConfiguration
    {
        public static void ConfigurePaymentServices(this IServiceCollection services, IConfiguration configuration)
        {
            var accessToken = configuration["MercadoPago:AccessToken"];

            if (!string.IsNullOrEmpty(accessToken))
            {
                MercadoPagoConfig.AccessToken = accessToken;
                Console.WriteLine("✅ Mercado Pago configurado correctamente.");
            }
            else
            {
                Console.WriteLine("⚠️ No se encontró el AccessToken de Mercado Pago en appsettings.json.");
            }
        }
    }
}
