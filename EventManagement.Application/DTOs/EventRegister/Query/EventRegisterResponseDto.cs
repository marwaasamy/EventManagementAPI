using EventManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Application.DTOs.EventRegister.Query
{
    public class EventRegisterResponseDto
    {
        public int Id { get; set; }
        public DateTime RegisterationDate { get; set; }
        public RegistrationStatus Status { get; set; }
        public int EventId { get; set; }
        public string EventTitle { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
