using EventManagement.Application.Common;
using EventManagement.Application.DTOs.Venue.Command;
using EventManagement.Application.DTOs.Venue.Query;
using EventManagement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.Interfaces
{
    public interface IVenueService
    {
        Task<ServiceResult<VenueDTO>> GetByIdAsync(int id);
        Task<ServiceResult<IEnumerable<VenueDTO>>> GetAllAsync();
        Task<ServiceResult<VenueDTO>> CreateAsync(CreateVenueDTO dto, string creatorUser);
        Task<ServiceResult<VenueDTO>> UpdateAsync(int id, UpdateVenueDTO dto, string modifierUser);
        Task<ServiceResult<bool>> DeleteAsync(int id, string deletedUser);
    }
}
