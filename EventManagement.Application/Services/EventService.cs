using System;
using System.Collections.Generic;
using System.Text;


    using AutoMapper;
    using global::EventManagement.Application.Common;
    using global::EventManagement.Application.DTOs.Event.Command;
    using global::EventManagement.Application.DTOs.Event.Query;
    using global::EventManagement.Application.Interfaces;
    using global::EventManagement.Domain.Entities;
    using global::EventManagement.Domain.Interfaces;
    using System.Linq.Expressions;

    namespace EventManagement.Application.Services
    {
        public class EventService : IEventService
        {
            private readonly IUnitOfWork _unitOfWork;
            private readonly IImageService _imageService;
            private readonly IMapper _mapper;

            public EventService(IUnitOfWork unitOfWork, IImageService imageService, IMapper mapper)
            {
                _unitOfWork = unitOfWork;
                _imageService = imageService;
                _mapper = mapper;
            }

            public async Task<ServiceResult<EventDto>> GetByIdAsync(int id)
            {
                var ev = await _unitOfWork.Events.GetAsync(
                    e => e.Id == id,
                    includes: new Expression<Func<Event, object>>[] { e => e.Category, e => e.Venue });

                if (ev is null)
                    return ServiceResult<EventDto>.Fail("Event not found");

                return ServiceResult<EventDto>.Ok(_mapper.Map<EventDto>(ev));
            }

            public async Task<ServiceResult<IEnumerable<EventListDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
            {
                var events = await _unitOfWork.Events.GetAllAsync(
                    includes: new Expression<Func<Event, object>>[] { e => e.Venue },
                    pageNumber: pageNumber,
                    pageSize: pageSize);

                return ServiceResult<IEnumerable<EventListDto>>.Ok(_mapper.Map<IEnumerable<EventListDto>>(events));
            }

            public async Task<ServiceResult<EventDto>> CreateAsync(CreateEventDto dto, Stream imageStream, string imageFileName, string creatorUser)
            {
                var imageUrl = await _imageService.AddImageAsync(imageStream, imageFileName, "events");
                if (imageUrl is null)
                    return ServiceResult<EventDto>.Fail("Failed to save image (invalid file type or size)");

                var ev = new Event(
                    dto.Title, dto.Capacity, dto.Description, dto.StartDate, dto.EndDate
                   , imageUrl, dto.Price, dto.IsPaid,
                    dto.CategoryId, dto.VenueId, dto.Status, creatorUser);

                try
                {
                    await _unitOfWork.Events.AddAsync(ev);
                    await _unitOfWork.CompleteAsync();
                }
                catch
                {
                    await _imageService.DeleteImageAsync(imageUrl);
                    throw;
                }

                return ServiceResult<EventDto>.Ok(_mapper.Map<EventDto>(ev), "Event created successfully");
            }

            public async Task<ServiceResult<EventDto>> UpdateAsync(int id, UpdateEventDto dto, Stream? imageStream, string? imageFileName, string modifierUser)
            {
                var ev = await _unitOfWork.Events.GetAsync(e => e.Id == id);
                if (ev is null)
                    return ServiceResult<EventDto>.Fail("Event not found");

                var imageUrl = ev.ImageUrl;
                string? oldImageUrl = null;

                if (imageStream is not null && !string.IsNullOrWhiteSpace(imageFileName))
                {
                    var newUrl = await _imageService.AddImageAsync(imageStream, imageFileName, "events");
                    if (newUrl is null)
                        return ServiceResult<EventDto>.Fail("Failed to save image (invalid file type or size)");

                    oldImageUrl = ev.ImageUrl;
                    imageUrl = newUrl;
                }

                ev.Update(dto.Title, dto.Capacity, dto.Description, dto.StartDate, dto.EndDate,
                    imageUrl, dto.Price, dto.IsPaid, dto.CategoryId, dto.VenueId, dto.Status, modifierUser);

                await _unitOfWork.Events.UpdateAsync(ev);
                await _unitOfWork.CompleteAsync();

                if (!string.IsNullOrWhiteSpace(oldImageUrl))
                    await _imageService.DeleteImageAsync(oldImageUrl);

                return ServiceResult<EventDto>.Ok(_mapper.Map<EventDto>(ev), "Event updated successfully");
            }

            public async Task<ServiceResult<bool>> DeleteAsync(int id, string deletedUser)
            {
                var ev = await _unitOfWork.Events.GetAsync(e => e.Id == id);
                if (ev is null)
                    return ServiceResult<bool>.Fail("Event not found");

                if (!string.IsNullOrWhiteSpace(ev.ImageUrl))
                    await _imageService.DeleteImageAsync(ev.ImageUrl);

                await _unitOfWork.Events.HardDeleteAsync(ev);
                await _unitOfWork.CompleteAsync();

                return ServiceResult<bool>.Ok(true, "Event deleted successfully");
            }
        }
    }

