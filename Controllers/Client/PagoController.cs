using MercadoPago.Client.Preference;
using MercadoPago.Resource.Preference;
using Microsoft.AspNetCore.Mvc;
using MercadoPago.Config;

namespace Hotel_chain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PagoController : ControllerBase
    {
        [HttpPost("crear-preferencia")]
        public async Task<IActionResult> CrearPreferencia([FromBody] PagoRequest request)
        {
            try
            {
                var client = new PreferenceClient();

                var preferenceRequest = new PreferenceRequest
                {
                    Items = new List<PreferenceItemRequest>
                    {
                        new PreferenceItemRequest
                        {
                            Title = request.Titulo,
                            Quantity = 1,
                            CurrencyId = "PEN",
                            UnitPrice = request.Monto
                        }
                    },
                    BackUrls = new PreferenceBackUrlsRequest
                    {
                        Success = "https://localhost:5105/Reservas/Exito",
                        Failure = "https://localhost:5105/Reservas/Error",
                        Pending = "https://localhost:5105/Reservas/Pendiente"
                    },
                    AutoReturn = "approved"
                };

                Preference preference = await client.CreateAsync(preferenceRequest);
                return Ok(new { preferenceId = preference.Id });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
    }

    public class PagoRequest
    {
        public string? Titulo { get; set; }
        public decimal Monto { get; set; }
    }
}
