using EventManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.DTOs.Event.Query
{
    public class EventListDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EventStatus Status { get; set; } 
        public string ImageUrl { get; set; } = string.Empty;
        public decimal? Price { get; set; }
        public bool IsPaid { get; set; }
        public string? VenueName { get; set; }
    }
}
