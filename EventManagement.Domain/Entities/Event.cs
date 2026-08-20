using EventManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; private set; }
        public int Capacity { get;private set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public EventStatus Status { get; private set; } = EventStatus.Available;
        public string ImageUrl { get; private set; }
        public decimal? Price { get; private set; }
        public bool IsPaid { get; private set; } // is the event paid or free
        public int CategoryId { get; private set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; private set; }
       
        public int VenueId { get; private set; }
        [ForeignKey("VenueId")]
        public Venue Venue { get; private set; }
        public ICollection<EventRegister> EventRegisters { get; private set; } = new List<EventRegister>();

        public Event(string title, int capacity, string description, DateTime startDate, DateTime endDate, string imageUrl,
            decimal? price, bool isPaid, int categoryId, int venueId, EventStatus status = EventStatus.Available, string creatorUser = "") : base(creatorUser)
        {
            Title = title;
            Capacity = capacity;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            ImageUrl = imageUrl;
            Price = price;
            IsPaid = isPaid;
            CategoryId = categoryId;
            VenueId = venueId;
        }

        public void Update(string title, int capacity, string description, DateTime startDate, DateTime endDate, string imageUrl,
            decimal? price, bool isPaid, int categoryId, int venueId, EventStatus status, string modifierUser = "")
        {
            Title = title;
            Capacity = capacity;
            Description = description;
            StartDate = startDate;
            EndDate = endDate;
            Status = status;
            ImageUrl = imageUrl;
            Price = price;
            IsPaid = isPaid;
            CategoryId = categoryId;
            VenueId = venueId;
            base.Update(modifierUser);
        }

        public override void Delete(string deletedUser = "")
        {
            base.Delete(deletedUser);
        }

        public void RegisterAttendee()
        {
            if (Status == EventStatus.SoldOut || Capacity <= 0)
                throw new InvalidOperationException("This event is sold out.");

            Capacity--;

            if (Capacity == 0)
                Status = EventStatus.SoldOut;
        }

        public void CancelAttendeeRegistration()
        {
            if (Status == EventStatus.SoldOut)
                Status = EventStatus.Available;

            Capacity++;
        }
    }
}
