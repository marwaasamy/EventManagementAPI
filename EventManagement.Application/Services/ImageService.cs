using EventManagement.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace EventManagement.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _env;
        private static readonly string[] AllowedExtensions = { ".jpg", ".jpeg", ".png", ".webp", ".gif" };
        private const long MaxFileSizeBytes = 5 * 1024 * 1024; // 5 MB

        public ImageService(IWebHostEnvironment env)
        {
            _env = env;
        }

        public async Task<string?> AddImageAsync(Stream imageStream, string fileName, string source)
        {
            var extension = Path.GetExtension(fileName).ToLowerInvariant();
            if (!AllowedExtensions.Contains(extension))
                return null;

            if (imageStream.Length > MaxFileSizeBytes)
                return null;

            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var folderRelative = Path.Combine("images", source);
            var folderAbsolute = Path.Combine(_env.WebRootPath, folderRelative);
            Directory.CreateDirectory(folderAbsolute);

            var filePathAbsolute = Path.Combine(folderAbsolute, uniqueFileName);

            using (var fileStream = new FileStream(filePathAbsolute, FileMode.Create))
            {
                await imageStream.CopyToAsync(fileStream);
            }

            return "/" + Path.Combine(folderRelative, uniqueFileName).Replace("\\", "/");
        }

        public Task<bool> DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrWhiteSpace(relativePath))
                return Task.FromResult(false);

            var trimmed = relativePath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar);
            var fullPath = Path.Combine(_env.WebRootPath, trimmed);

            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            File.Delete(fullPath);
            return Task.FromResult(true);
        }
    }
}