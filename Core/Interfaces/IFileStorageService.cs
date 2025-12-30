
namespace EquipmentShop.Core.Interfaces
{
    public interface IFileStorageService
    {
        Task<string> SaveProductImageAsync(Stream fileStream, string fileName);
        Task<string> SaveCategoryImageAsync(Stream fileStream, string fileName);
        Task<string> SaveUserAvatarAsync(Stream fileStream, string fileName);
        Task<bool> DeleteFileAsync(string filePath);
        Task<Stream> GetFileAsync(string filePath);
        Task<string> GenerateUniqueFileName(string originalFileName);
        Task<IEnumerable<string>> GetProductGalleryAsync(int productId);
        Task ClearProductGalleryAsync(int productId);
    }

    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string FilePath { get; set; } = string.Empty;
        public string FileName { get; set; } = string.Empty;
        public long FileSize { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
