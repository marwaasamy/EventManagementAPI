using EventManagement.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Infrastructure.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ImageService(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<string?> AddImageAsync(Stream imageStream, string fileName, string source)
        {
            if (imageStream is null || imageStream.Length == 0)
                return null;

            var rootPath = _webHostEnvironment.WebRootPath;
            var uploadsPath = Path.Combine(rootPath, "Uploads", source);

            if (!Directory.Exists(uploadsPath))
                Directory.CreateDirectory(uploadsPath);

            var uniqueFileName = Guid.NewGuid() + Path.GetExtension(fileName);
            var filePath = Path.Combine(uploadsPath, uniqueFileName);

            using (var fs = new FileStream(filePath, FileMode.Create))
            {
                await imageStream.CopyToAsync(fs);
            }

            return $"/{Path.Combine("Uploads", source, uniqueFileName).Replace("\\", "/")}";
        }

        public Task<bool> DeleteImageAsync(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return Task.FromResult(false);

            if (relativePath.StartsWith("/"))
                relativePath = relativePath[1..];

            var fullPath = Path.Combine(_webHostEnvironment.WebRootPath, relativePath);

            if (!File.Exists(fullPath))
                return Task.FromResult(false);

            File.Delete(fullPath);
            return Task.FromResult(true);
        }
    }
}

