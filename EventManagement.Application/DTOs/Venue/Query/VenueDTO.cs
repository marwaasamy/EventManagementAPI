using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.DTOs.Venue.Query
{
    public class VenueDTO
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string City { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public int Capacity { get; set; }
        public string PhoneNumber { get; set; } = string.Empty;
    }
}
