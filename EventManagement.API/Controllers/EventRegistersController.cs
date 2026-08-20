// API/Controllers/EventRegistersController.cs
using EventManagement.Application.DTOs.EventRegister;
using EventManagement.Application.DTOs.EventRegister.Command;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Constants; // Roles.Admin
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EventManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class EventRegistersController : ControllerBase
    {
        private readonly IEventRegisterService _eventRegisterService;

        public EventRegistersController(IEventRegisterService eventRegisterService)
        {
            _eventRegisterService = eventRegisterService;
        }

        private string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier) ?? string.Empty;

        [HttpPost]
        public async Task<IActionResult> Register([FromBody] EventRegisterCreateDto dto)
        {
            var result = await _eventRegisterService.RegisterAsync(dto.EventId, CurrentUserId, CurrentUserId);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _eventRegisterService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpGet("my")]
        public async Task<IActionResult> GetMyRegistrations()
        {
            var result = await _eventRegisterService.GetByUserIdAsync(CurrentUserId);
            return Ok(result);
        }

        [HttpGet("event/{eventId:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetByEvent(int eventId)
        {
            var result = await _eventRegisterService.GetByEventIdAsync(eventId);
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> GetAll(int pageNumber = 1, int pageSize = 10)
        {
            var result = await _eventRegisterService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Cancel(int id)
        {
            var isAdmin = User.IsInRole(Roles.Admin);
            var result = await _eventRegisterService.CancelRegistrationAsync(id, CurrentUserId, isAdmin);
            return result.Success ? Ok(result) : BadRequest(result);
        }
    }
}