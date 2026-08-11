using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public ICollection<Event> Events { get; private set; } = new List<Event>();

        public Category(string name, string description, string creatorUser = "") : base(creatorUser)
        {
            Name = name;
            Description = description;
        }

        public void Update(string name, string description, string modifierUser = "")
        {
            Name = name;
            Description = description;
            base.Update(modifierUser);
        }

        public override void Delete(string deletedUser = "")
        {
            base.Delete(deletedUser);
        }
    }
}
