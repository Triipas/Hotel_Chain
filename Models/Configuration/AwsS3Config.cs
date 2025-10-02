// Models/Configuration/AwsS3Config.cs
namespace Hotel_chain.Models.Configuration
{
    public class AwsS3Config
    {
        public string AccessKey { get; set; } = string.Empty;
        public string SecretKey { get; set; } = string.Empty;
        public string Region { get; set; } = "us-east-2";
        public string BucketName { get; set; } = string.Empty;
        public string BucketUrl { get; set; } = string.Empty;
    }
}