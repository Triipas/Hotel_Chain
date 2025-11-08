using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using Hotel_chain.Services.Interfaces;
using System.Collections.Concurrent;

namespace Hotel_chain.Services.Implementation
{
    public class ChatService : IChatService
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatService;
        
        private static readonly ConcurrentDictionary<string, ChatHistory> _userHistories = new();
        
        private readonly string _systemPrompt = @"Eres un asistente virtual para 'Costa Dorada', un sistema de reservas de hoteles en Perú.

TU TRABAJO:
- Ayudar a los usuarios a buscar hoteles y habitaciones
- Proporcionar información sobre disponibilidad y precios
- Responder preguntas sobre servicios, amenidades y políticas
- Ser amigable, profesional y conciso

IMPORTANTE:
- Siempre usa las funciones disponibles para obtener información actualizada de la base de datos
- Si el usuario pide buscar algo, usa las funciones de búsqueda
- Si no tienes información, sé honesto y ofrece ayuda alternativa
- Responde en español de forma clara y estructurada
- Usa emojis ocasionalmente para hacer las respuestas más amigables

FUNCIONES DISPONIBLES:
- BuscarHoteles: busca hoteles por ciudad, nombre o amenidades
- BuscarHabitaciones: busca habitaciones en un hotel con sus amenidades
- ListarAmenidadesHotel: muestra todas las amenidades de un hotel
- BuscarHotelesPorAmenidades: encuentra hoteles que tengan amenidades específicas
- ObtenerDetallesHotel: información detallada de un hotel incluyendo amenidades
- VerificarDisponibilidad: revisa disponibilidad para fechas específicas con amenidades
- ListarTodosLosHoteles: muestra todos los hoteles
- ObtenerAyuda: muestra información de ayuda";

        public ChatService(
            IConfiguration configuration,
            IHotelService hotelService,
            IHabitacionService habitacionService,
            IReservaService reservaService,
            IHotelAmenidadService hotelAmenidadService,
            IHabitacionAmenidadService habitacionAmenidadService)
        {
            var apiKey = configuration["OpenAI:ApiKey"];
            var modelId = configuration["OpenAI:ModelId"] ?? "gpt-4o-mini";

            var builder = Kernel.CreateBuilder();
            builder.AddOpenAIChatCompletion(modelId, apiKey);
            
            var hotelPlugin = new HotelPlugin(
                hotelService, 
                habitacionService, 
                reservaService, 
                hotelAmenidadService, 
                habitacionAmenidadService);
            
            builder.Plugins.AddFromObject(hotelPlugin, "HotelPlugin");
            
            _kernel = builder.Build();
            _chatService = _kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> GetResponseAsync(string userMessage, string? userId = null)
        {
            try
            {
                var sessionId = userId ?? "guest";
                
                var chatHistory = _userHistories.GetOrAdd(sessionId, _ => new ChatHistory(_systemPrompt));

                chatHistory.AddUserMessage(userMessage);

                var executionSettings = new OpenAIPromptExecutionSettings
                {
                    ToolCallBehavior = ToolCallBehavior.AutoInvokeKernelFunctions,
                    Temperature = 0.7,
                    MaxTokens = 800
                };

                var response = await _chatService.GetChatMessageContentAsync(
                    chatHistory,
                    executionSettings,
                    _kernel);

                chatHistory.AddAssistantMessage(response.Content ?? "");

                while (chatHistory.Count > 20)
                {
                    chatHistory.RemoveAt(1);
                }

                return response.Content ?? "Lo siento, no pude generar una respuesta.";
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }
        
        public static void LimpiarHistorialesViejos()
        {
            // Implementación opcional para limpiar historiales antiguos
        }
    }
}