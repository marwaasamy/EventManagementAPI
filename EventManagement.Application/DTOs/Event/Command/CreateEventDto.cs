using EventManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.DTOs.Event.Command
{
    public class CreateEventDto
    {
        public string Title { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string Description { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus Status { get; set; } 
        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }
        public int CategoryId { get; set; }
        public int VenueId { get; set; }
        // Image itself is passed separately as IFormFile — see controller
    }
}
