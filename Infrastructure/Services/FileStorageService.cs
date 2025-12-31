using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using EquipmentShop.Core.Interfaces;

namespace EquipmentShop.Infrastructure.Services
{
    public class FileStorageService : IFileStorageService
    {
        private readonly ILogger<FileStorageService> _logger;
        private readonly string _storagePath;

        public FileStorageService(ILogger<FileStorageService> logger)
        {
            _logger = logger;
            // Указываем путь для хранения файлов
            _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");

            // Создаем папку если не существует
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }
        }

        public async Task<string> SaveProductImageAsync(Stream fileStream, string fileName)
        {
            return await SaveFileAsync(fileStream, fileName, "products");
        }

        public async Task<string> SaveCategoryImageAsync(Stream fileStream, string fileName)
        {
            return await SaveFileAsync(fileStream, fileName, "categories");
        }

        public async Task<string> SaveUserAvatarAsync(Stream fileStream, string fileName)
        {
            return await SaveFileAsync(fileStream, fileName, "avatars");
        }

        private async Task<string> SaveFileAsync(Stream fileStream, string fileName, string subFolder)
        {
            try
            {
                var folderPath = Path.Combine(_storagePath, subFolder);
                Directory.CreateDirectory(folderPath);

                var uniqueFileName = $"{Guid.NewGuid()}{Path.GetExtension(fileName)}";
                var filePath = Path.Combine(folderPath, uniqueFileName);

                using (var file = new FileStream(filePath, FileMode.Create))
                {
                    await fileStream.CopyToAsync(file);
                }

                return $"/uploads/{subFolder}/{uniqueFileName}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при сохранении файла");
                throw;
            }
        }

        public Task<bool> DeleteFileAsync(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return Task.FromResult(false);

                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return Task.FromResult(true);
                }

                return Task.FromResult(false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении файла");
                return Task.FromResult(false);
            }
        }

        public Task<Stream> GetFileAsync(string filePath)
        {
            try
            {
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", filePath.TrimStart('/'));

                if (!File.Exists(fullPath))
                    throw new FileNotFoundException("Файл не найден");

                return Task.FromResult<Stream>(new FileStream(fullPath, FileMode.Open, FileAccess.Read));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении файла");
                throw;
            }
        }

        public Task<string> GenerateUniqueFileName(string originalFileName)
        {
            var extension = Path.GetExtension(originalFileName);
            return Task.FromResult($"{Guid.NewGuid()}{extension}");
        }

        public Task<IEnumerable<string>> GetProductGalleryAsync(int productId)
        {
            // Заглушка для демо
            return Task.FromResult<IEnumerable<string>>(new List<string>());
        }

        public Task ClearProductGalleryAsync(int productId)
        {
            // Заглушка для демо
            return Task.CompletedTask;
        }
    }
}