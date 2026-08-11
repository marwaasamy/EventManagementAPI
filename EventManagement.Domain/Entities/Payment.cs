using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public class Payment : BaseEntity
    {
        public decimal Amount { get; private set; }
        public DateTime PaymentDate { get; private set; }
        public string PaymentMethod { get; private set; }
        public int EventRegisterId { get; private set; }
        [ForeignKey("EventRegisterId")]
        public EventRegister EventRegister { get; private set; }

        public Payment(decimal amount, DateTime paymentDate, string paymentMethod, int eventRegisterId, string creatorUser = "") : base(creatorUser)
        {
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            EventRegisterId = eventRegisterId;
        }

        public void Update(decimal amount, DateTime paymentDate, string paymentMethod, int eventRegisterId, string modifierUser = "")
        {
            Amount = amount;
            PaymentDate = paymentDate;
            PaymentMethod = paymentMethod;
            EventRegisterId = eventRegisterId;
            base.Update(modifierUser);
        }

        public override void Delete(string deletedUser = "")
        {
            base.Delete(deletedUser);
        }
    }
}
