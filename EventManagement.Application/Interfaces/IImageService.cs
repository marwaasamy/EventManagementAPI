using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface IImageService
    {
        Task<string?> AddImageAsync(Stream imageStream, string fileName, string source);
        Task<bool> DeleteImageAsync(string relativePath);
    }
}
