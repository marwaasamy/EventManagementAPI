using AutoMapper;
using EventManagement.Application.Common;
using EventManagement.Application.DTOs.Venue.Command;
using EventManagement.Application.DTOs.Venue.Query;
using EventManagement.Domain.Interfaces;
using EventManagement.Domain.Entities;
using EventManagement.Application.Interfaces;

namespace EventManagement.Application.Services
{
    public class VenueService : IVenueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public VenueService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<VenueDTO>> GetByIdAsync(int id)
        {
            var venue = await _unitOfWork.Venues.GetAsync(v => v.Id == id);
            if (venue == null)
                return ServiceResult<VenueDTO>.Fail("Venue not found");

            return ServiceResult<VenueDTO>.Ok(_mapper.Map<VenueDTO>(venue));
        }

        public async Task<ServiceResult<IEnumerable<VenueDTO>>> GetAllAsync()
        {
            var venues = await _unitOfWork.Venues.GetAllAsync();
            var dtos = _mapper.Map<IEnumerable<VenueDTO>>(venues);

            return ServiceResult<IEnumerable<VenueDTO>>.Ok(dtos);
        }

        public async Task<ServiceResult<VenueDTO>> CreateAsync(CreateVenueDTO dto, string creatorUser)
        {
            var venue = new Venue(dto.Name, dto.City, dto.Address, dto.Capacity, dto.PhoneNumber, creatorUser);

            await _unitOfWork.Venues.AddAsync(venue);
            var saved = await _unitOfWork.CompleteAsync();

            if (saved <= 0)
                return ServiceResult<VenueDTO>.Fail("Failed to create venue");

            return ServiceResult<VenueDTO>.Ok(_mapper.Map<VenueDTO>(venue), "Venue created successfully");
        }

        public async Task<ServiceResult<VenueDTO>> UpdateAsync(int id, UpdateVenueDTO dto, string modifierUser)
        {
            var venue = await _unitOfWork.Venues.GetAsync(v => v.Id == id);
            if (venue == null)
                return ServiceResult<VenueDTO>.Fail("Venue not found");

            venue.Update(dto.Name, dto.City, dto.Address, dto.Capacity, dto.PhoneNumber, modifierUser);
            await _unitOfWork.Venues.UpdateAsync(venue);

            var saved = await _unitOfWork.CompleteAsync();
            if (saved <= 0)
                return ServiceResult<VenueDTO>.Fail("Failed to update venue");

            return ServiceResult<VenueDTO>.Ok(_mapper.Map<VenueDTO>(venue), "Venue updated successfully");
        }

        public async Task<ServiceResult<bool>> DeleteAsync(int id, string deletedUser)
        {
            var venue = await _unitOfWork.Venues.GetAsync(v => v.Id == id);
            if (venue == null)
                return ServiceResult<bool>.Fail("Venue not found");

            await _unitOfWork.Venues.SoftDeleteAsync(venue, deletedUser);

            var saved = await _unitOfWork.CompleteAsync();
            if (saved <= 0)
                return ServiceResult<bool>.Fail("Failed to delete venue");

            return ServiceResult<bool>.Ok(true, "Venue deleted successfully");
        }
    }
}