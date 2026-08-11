using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public class Venue : BaseEntity
    {
        public string Name { get; private set; }
        public string City { get; private set; }
        public string Address { get; private set; }
        public int Capacity { get; private set; }
        public string PhoneNumber { get; private set; }
        public string ImageUrl { get; private set; }
        public ICollection<Event> Events { get; private set; } = new List<Event>();

        public Venue(string name, string city, string address, int capacity, string phoneNumber, string imageUrl, string creatorUser = "") : base(creatorUser)
        {
            Name = name;
            City = city;
            Address = address;
            Capacity = capacity;
            PhoneNumber = phoneNumber;
            ImageUrl = imageUrl;
        }

        public void Update(string name, string city, string address, int capacity, string phoneNumber, string imageUrl, string modifierUser = "")
        {
            Name = name;
            City = city;
            Address = address;
            Capacity = capacity;
            PhoneNumber = phoneNumber;
            ImageUrl = imageUrl;
            base.Update(modifierUser);
        }

        public override void Delete(string deletedUser = "")
        {
            base.Delete(deletedUser);
        }
    }
}
