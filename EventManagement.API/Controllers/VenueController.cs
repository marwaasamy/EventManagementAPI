using EventManagement.Application.DTOs.Venue.Command;
using EventManagement.Application.Interfaces;
using EventManagement.Domain.Constants;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VenueController : ControllerBase
    {
        private readonly IVenueService _venueService;

        public VenueController(IVenueService venueService)
        {
            _venueService = venueService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _venueService.GetAllAsync();
            return result.Success ? Ok(result.Data) : BadRequest(result.Message);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _venueService.GetByIdAsync(id);
            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpPost]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Create([FromBody] CreateVenueDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var creatorUser = User.Identity?.Name ?? "system";
            var result = await _venueService.CreateAsync(dto, creatorUser);

            return result.Success
                ? CreatedAtAction(nameof(GetById), new { id = result.Data!.Id }, result.Data)
                : BadRequest(result.Message);
        }

        [HttpPut("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVenueDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var modifierUser = User.Identity?.Name ?? "system";
            var result = await _venueService.UpdateAsync(id, dto, modifierUser);

            return result.Success ? Ok(result.Data) : NotFound(result.Message);
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = Roles.Admin)]
        public async Task<IActionResult> Delete(int id)
        {
            var deletedUser = User.Identity?.Name ?? "system";
            var result = await _venueService.DeleteAsync(id, deletedUser);

            return result.Success ? Ok(result.Message) : NotFound(result.Message);
        }
    }
}
