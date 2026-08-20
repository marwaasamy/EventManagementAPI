using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public class Event : BaseEntity
    {
        public string Title { get; private set; }
        public int Capacity { get; set; }
        public string Description { get; private set; }
        public DateTime StartDate { get; private set; }
        public DateTime EndDate { get; private set; }
        public string Status { get; private set; }
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

        public Event(string title, int capacity, string description, DateTime startDate, DateTime endDate, string status, string imageUrl,
            decimal? price, bool isPaid, int categoryId, int venueId, string creatorUser = "") : base(creatorUser)
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

        public void Update(string title, int capacity, string description, DateTime startDate, DateTime endDate, string status, string imageUrl,
            decimal? price, bool isPaid, int categoryId, int venueId, string modifierUser = "")
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
    }
}
