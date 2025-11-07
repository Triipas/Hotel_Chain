using Microsoft.AspNetCore.Mvc;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly IChatService _chatService;

        public ChatController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpPost("message")]
        public async Task<IActionResult> SendMessage([FromBody] ChatRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest(new { error = "El mensaje no puede estar vacío" });
            }

            try
            {
                // 🔹 Obtener ID de sesión o usuario
                var userId = HttpContext.Session.GetString("UsuarioId") 
                    ?? HttpContext.Session.Id; // Usar SessionId si no está logueado

                var response = await _chatService.GetResponseAsync(request.Message, userId);
                return Ok(new { response });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = ex.Message });
            }
        }
    }

    public class ChatRequest
    {
        public string Message { get; set; } = string.Empty;
    }
}