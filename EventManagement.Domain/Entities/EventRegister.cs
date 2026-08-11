using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public class EventRegister : BaseEntity
    {
        public DateTime RegisterationDate { get; private set; }
        public string Status { get; private set; }
        public int EventId { get; private set; }
        [ForeignKey("EventId")]
        public Event Event { get; private set; }
        public string UserId { get; private set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; private set; }

        public ICollection<Payment> Payments { get; private set; } = new List<Payment>();

        public EventRegister(DateTime registerationDate, string status, int eventId, string userId, string creatorUser = "") : base(creatorUser)
        {
            RegisterationDate = registerationDate;
            Status = status;
            EventId = eventId;
            UserId = userId;
        }

        public void Update(DateTime registerationDate, string status, int eventId, string userId, string modifierUser = "")
        {
            RegisterationDate = registerationDate;
            Status = status;
            EventId = eventId;
            UserId = userId;
            base.Update(modifierUser);
        }

        public override void Delete(string deletedUser = "")
        {
            base.Delete(deletedUser);
        }
    }
}
