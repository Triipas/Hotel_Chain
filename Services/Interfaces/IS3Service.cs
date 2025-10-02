// Services/Interfaces/IS3Service.cs
namespace Hotel_chain.Services.Interfaces
{
    public interface IS3Service
    {
        Task<string> UploadFileAsync(IFormFile file, string folder, string? customFileName = null);
        Task<bool> DeleteFileAsync(string fileUrl);
        Task<List<string>> UploadMultipleFilesAsync(List<IFormFile> files, string folder);
        string GetFileUrl(string fileName, string folder);
        Task<bool> FileExistsAsync(string fileUrl);
    }
}