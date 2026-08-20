// Application/Interfaces/IEventRegisterService.cs
using EventManagement.Application.Common;
using EventManagement.Application.DTOs.EventRegister;
using EventManagement.Application.DTOs.EventRegister.Query;

namespace EventManagement.Application.Interfaces
{
    public interface IEventRegisterService
    {
        Task<ServiceResult<EventRegisterResponseDto>> RegisterAsync(int eventId, string userId, string creatorUser = "");
        Task<ServiceResult<EventRegisterResponseDto>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<EventRegisterResponseDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10);
        Task<ServiceResult<IEnumerable<EventRegisterResponseDto>>> GetByUserIdAsync(string userId);
        Task<ServiceResult<IEnumerable<EventRegisterResponseDto>>> GetByEventIdAsync(int eventId);
        Task<ServiceResult<bool>> CancelRegistrationAsync(int registerId, string userId, bool isAdmin = false, string modifierUser = "");
    }
}