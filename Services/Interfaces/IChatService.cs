namespace Hotel_chain.Services.Interfaces
{
    public interface IChatService
    {
        Task<string> GetResponseAsync(string userMessage, string? userId = null);
    }
}