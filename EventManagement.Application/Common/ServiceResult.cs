using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.Common
{
    public class ServiceResult<T>
    {
        public bool Success { get; private set; }
        public string? Message { get; private set; }
        public T? Data { get; private set; }

        public static ServiceResult<T> Ok(T data, string? message = null) =>
            new() { Success = true, Data = data, Message = message };
        
        public static ServiceResult<T> Fail(string message) =>
            new() { Success = false, Message = message };
    }
}
