using EventManagement.Application.DTOs.Event;
using EventManagement.Application.DTOs.Event.Command;
using EventManagement.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventsController(IEventService eventService)
        {
            _eventService = eventService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var result = await _eventService.GetAllAsync(pageNumber, pageSize);
            return Ok(result);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _eventService.GetByIdAsync(id);
            return result.Success ? Ok(result) : NotFound(result);
        }

        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> Create([FromForm] CreateEventDto dto, IFormFile image)
        {
            if (image is null || image.Length == 0)
                return BadRequest("An event image is required");

            var creatorUser = User.Identity?.Name ?? "system";

            await using var stream = image.OpenReadStream();
            var result = await _eventService.CreateAsync(dto, stream, image.FileName, creatorUser);

            return result.Success
                ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result)
                : BadRequest(result);
        }

        [HttpPut("{id:int}")]
        //[Authorize(Roles = "Admin")]
        [RequestSizeLimit(5 * 1024 * 1024)]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateEventDto dto, IFormFile? image)
        {
            var modifierUser = User.Identity?.Name ?? "system";

            if (image is not null && image.Length > 0)
            {
                await using var stream = image.OpenReadStream();
                var resultWithImage = await _eventService.UpdateAsync(id, dto, stream, image.FileName, modifierUser);
                return resultWithImage.Success ? Ok(resultWithImage) : BadRequest(resultWithImage);
            }

            var result = await _eventService.UpdateAsync(id, dto, null, null, modifierUser);
            return result.Success ? Ok(result) : BadRequest(result);
        }

        [HttpDelete("{id:int}")]
        //[Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedUser = User.Identity?.Name ?? "system";
            var result = await _eventService.DeleteAsync(id, deletedUser);
            return result.Success ? Ok(result) : NotFound(result);
        }
    }
}