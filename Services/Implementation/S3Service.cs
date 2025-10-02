// Services/Implementation/S3Service.cs
using Amazon.S3;
using Amazon.S3.Model;
using Amazon.S3.Transfer;
using Microsoft.Extensions.Options;
using Hotel_chain.Models.Configuration;
using Hotel_chain.Services.Interfaces;

namespace Hotel_chain.Services.Implementation
{
    public class S3Service : IS3Service
    {
        private readonly IAmazonS3 _s3Client;
        private readonly AwsS3Config _config;
        private readonly ILogger<S3Service> _logger;

        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxFileSize = 5 * 1024 * 1024; // 5 MB

        public S3Service(
            IAmazonS3 s3Client,
            IOptions<AwsS3Config> config,
            ILogger<S3Service> logger)
        {
            _s3Client = s3Client;
            _config = config.Value;
            _logger = logger;
        }

        /// <summary>
        /// Sube un archivo a S3
        /// </summary>
        public async Task<string> UploadFileAsync(IFormFile file, string folder, string? customFileName = null)
        {
            try
            {
                // Validaciones
                ValidateFile(file);

                // Generar nombre único
                var fileName = customFileName ?? GenerateUniqueFileName(file.FileName);
                var key = $"{folder}/{fileName}";

                // Configurar la transferencia
                var transferUtility = new TransferUtility(_s3Client);
                var uploadRequest = new TransferUtilityUploadRequest
                {
                    InputStream = file.OpenReadStream(),
                    Key = key,
                    BucketName = _config.BucketName,
                    ContentType = file.ContentType,
                    CannedACL = S3CannedACL.PublicRead // Hace la imagen pública
                };

                // Subir archivo
                await transferUtility.UploadAsync(uploadRequest);

                // Retornar URL completa
                var fileUrl = $"{_config.BucketUrl}/{key}";
                
                _logger.LogInformation($"Archivo subido exitosamente: {fileUrl}");
                
                return fileUrl;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al subir archivo a S3: {file.FileName}");
                throw new Exception($"Error al subir imagen: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un archivo de S3
        /// </summary>
        public async Task<bool> DeleteFileAsync(string fileUrl)
        {
            try
            {
                if (string.IsNullOrEmpty(fileUrl))
                    return false;

                // Extraer el key del URL
                var key = ExtractKeyFromUrl(fileUrl);
                
                if (string.IsNullOrEmpty(key))
                    return false;

                var deleteRequest = new DeleteObjectRequest
                {
                    BucketName = _config.BucketName,
                    Key = key
                };

                await _s3Client.DeleteObjectAsync(deleteRequest);
                
                _logger.LogInformation($"Archivo eliminado exitosamente: {fileUrl}");
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error al eliminar archivo de S3: {fileUrl}");
                return false;
            }
        }

        /// <summary>
        /// Sube múltiples archivos a S3
        /// </summary>
        public async Task<List<string>> UploadMultipleFilesAsync(List<IFormFile> files, string folder)
        {
            var urls = new List<string>();

            foreach (var file in files)
            {
                try
                {
                    var url = await UploadFileAsync(file, folder);
                    urls.Add(url);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Error al subir archivo: {file.FileName}");
                    // Continuar con los demás archivos
                }
            }

            return urls;
        }

        /// <summary>
        /// Obtiene la URL completa de un archivo
        /// </summary>
        public string GetFileUrl(string fileName, string folder)
        {
            return $"{_config.BucketUrl}/{folder}/{fileName}";
        }

        /// <summary>
        /// Verifica si un archivo existe en S3
        /// </summary>
        public async Task<bool> FileExistsAsync(string fileUrl)
        {
            try
            {
                var key = ExtractKeyFromUrl(fileUrl);
                
                if (string.IsNullOrEmpty(key))
                    return false;

                var request = new GetObjectMetadataRequest
                {
                    BucketName = _config.BucketName,
                    Key = key
                };

                await _s3Client.GetObjectMetadataAsync(request);
                return true;
            }
            catch (AmazonS3Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Valida el archivo antes de subirlo
        /// </summary>
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("El archivo está vacío");

            if (file.Length > MaxFileSize)
                throw new ArgumentException($"El archivo excede el tamaño máximo permitido de {MaxFileSize / 1024 / 1024} MB");

            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            
            if (!AllowedExtensions.Contains(extension))
                throw new ArgumentException($"Formato de archivo no permitido. Formatos válidos: {string.Join(", ", AllowedExtensions)}");
        }

        /// <summary>
        /// Genera un nombre de archivo único
        /// </summary>
        private string GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            var timestamp = DateTime.UtcNow.ToString("yyyyMMdd_HHmmss");
            var guid = Guid.NewGuid().ToString("N").Substring(0, 8);
            
            return $"{timestamp}_{guid}{extension}";
        }

        /// <summary>
        /// Extrae el key (ruta) del URL de S3
        /// </summary>
        private string ExtractKeyFromUrl(string fileUrl)
        {
            if (string.IsNullOrEmpty(fileUrl))
                return string.Empty;

            // Ejemplo: https://staygo-imagenes.s3.us-east-2.amazonaws.com/hoteles/1/image.jpg
            // Resultado: hoteles/1/image.jpg
            
            var uri = new Uri(fileUrl);
            return uri.AbsolutePath.TrimStart('/');
        }
    }
}