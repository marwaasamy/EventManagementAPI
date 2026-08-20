// Application/Services/EventRegisterService.cs
using AutoMapper;
using EventManagement.Application.Common;
using EventManagement.Application.DTOs.EventRegister;
using EventManagement.Application.DTOs.EventRegister.Query;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Constants;
using EventManagement.Domain.Entities;
using EventManagement.Domain.Enums;
using EventManagement.Domain.Interfaces;
using System.Linq.Expressions;

namespace EventManagement.Application.Services
{
    public class EventRegisterService : IEventRegisterService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public EventRegisterService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<ServiceResult<EventRegisterResponseDto>> RegisterAsync(int eventId, string userId, string creatorUser = "")
        {
            var ev = await _unitOfWork.Events.GetAsync(e => e.Id == eventId);
            if (ev is null)
                return ServiceResult<EventRegisterResponseDto>.Fail("Event not found");

            var alreadyRegistered = await _unitOfWork.EventRegisters.GetAsync(
                r => r.EventId == eventId && r.UserId == userId && r.Status != RegistrationStatus.Cancelled);
            if (alreadyRegistered is not null)
                return ServiceResult<EventRegisterResponseDto>.Fail("You are already registered for this event");

            try
            {
                ev.RegisterAttendee(); // throws if sold out
            }
            catch (InvalidOperationException ex)
            {
                return ServiceResult<EventRegisterResponseDto>.Fail(ex.Message);
            }

            var register = new EventRegister(DateTime.UtcNow, RegistrationStatus.Registered, eventId, userId, creatorUser);

            await _unitOfWork.EventRegisters.AddAsync(register);
            await _unitOfWork.Events.UpdateAsync(ev);
            await _unitOfWork.CompleteAsync();

            var saved = await _unitOfWork.EventRegisters.GetAsync(
                r => r.Id == register.Id,
                includes: new Expression<Func<EventRegister, object>>[] { r => r.Event, r => r.User });

            return ServiceResult<EventRegisterResponseDto>.Ok(_mapper.Map<EventRegisterResponseDto>(saved), "Registered successfully");
        }

        public async Task<ServiceResult<EventRegisterResponseDto>> GetByIdAsync(int id)
        {
            var register = await _unitOfWork.EventRegisters.GetAsync(
                r => r.Id == id,
                includes: new Expression<Func<EventRegister, object>>[] { r => r.Event, r => r.User });

            if (register is null)
                return ServiceResult<EventRegisterResponseDto>.Fail("Registration not found");

            return ServiceResult<EventRegisterResponseDto>.Ok(_mapper.Map<EventRegisterResponseDto>(register));
        }

        public async Task<ServiceResult<IEnumerable<EventRegisterResponseDto>>> GetAllAsync(int pageNumber = 1, int pageSize = 10)
        {
            var registers = await _unitOfWork.EventRegisters.GetAllAsync(
                includes: new Expression<Func<EventRegister, object>>[] { r => r.Event, r => r.User },
                pageNumber: pageNumber,
                pageSize: pageSize);

            return ServiceResult<IEnumerable<EventRegisterResponseDto>>.Ok(_mapper.Map<IEnumerable<EventRegisterResponseDto>>(registers));
        }

        public async Task<ServiceResult<IEnumerable<EventRegisterResponseDto>>> GetByUserIdAsync(string userId)
        {
            var registers = await _unitOfWork.EventRegisters.GetAllAsync(
                r => r.UserId == userId,
                includes: new Expression<Func<EventRegister, object>>[] { r => r.Event, r => r.User });

            return ServiceResult<IEnumerable<EventRegisterResponseDto>>.Ok(_mapper.Map<IEnumerable<EventRegisterResponseDto>>(registers));
        }

        public async Task<ServiceResult<IEnumerable<EventRegisterResponseDto>>> GetByEventIdAsync(int eventId)
        {
            var registers = await _unitOfWork.EventRegisters.GetAllAsync(
                r => r.EventId == eventId,
                includes: new Expression<Func<EventRegister, object>>[] { r => r.Event, r => r.User });

            return ServiceResult<IEnumerable<EventRegisterResponseDto>>.Ok(_mapper.Map<IEnumerable<EventRegisterResponseDto>>(registers));
        }

        public async Task<ServiceResult<bool>> CancelRegistrationAsync(int registerId, string userId, bool isAdmin = false, string modifierUser = "")
        {
            var register = await _unitOfWork.EventRegisters.GetAsync(r => r.Id == registerId);
            if (register is null)
                return ServiceResult<bool>.Fail("Registration not found");

            if (!isAdmin && register.UserId != userId)
                return ServiceResult<bool>.Fail("You can only cancel your own registration");

            if (register.Status == RegistrationStatus.Cancelled)
                return ServiceResult<bool>.Fail("Registration is already cancelled");

            var ev = await _unitOfWork.Events.GetAsync(e => e.Id == register.EventId);
            if (ev is not null)
            {
                ev.CancelAttendeeRegistration();
                await _unitOfWork.Events.UpdateAsync(ev);
            }

            await _unitOfWork.EventRegisters.HardDeleteAsync(register);

            await _unitOfWork.CompleteAsync();

            return ServiceResult<bool>.Ok(true, "Registration cancelled successfully");
        }
    }
}