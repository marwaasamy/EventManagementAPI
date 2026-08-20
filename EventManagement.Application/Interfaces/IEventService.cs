using EventManagement.Application.Common;
using EventManagement.Application.DTOs.Event.Command;
using EventManagement.Application.DTOs.Event.Query;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface IEventService
    {
        Task<ServiceResult<EventDto>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<EventListDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10);

        Task<ServiceResult<EventDto>> CreateAsync(CreateEventDto dto, Stream imageStream, string imageFileName, string creatorUser);

        Task<ServiceResult<EventDto>> UpdateAsync(int id, UpdateEventDto dto, Stream? imageStream, string? imageFileName, string modifierUser);

        Task<ServiceResult<bool>> DeleteAsync(int id, string deletedUser);
    }
}
