using System;
using System.Collections.Generic;
using System.Text;

namespace EventManagement.Domain.Entities
{
    public abstract class BaseEntity
    {
        public int Id { get; private set; }
        public string? CreatorUser { get; private set; }

        public DateTime CreatedOn { get; private set; }

        public string? ModifierUser { get; private set; }
        public DateTime? ModifiedOn { get; private set; }

        public bool IsDeleted { get; private set; }
        public string? DeletedUser { get; private set; }
        public DateTime? DeletedOn { get; private set; }

        public BaseEntity(string creatorUser = "")
        {
            CreatorUser = creatorUser;
            CreatedOn = DateTime.Now;
            IsDeleted = false;
        }

        public virtual void Update(string modifierUser = "")
        {
            ModifierUser = modifierUser;
            ModifiedOn = DateTime.Now;
        }

        public virtual void Delete(string deletedUser = "")
        {
            IsDeleted = true;
            DeletedUser = deletedUser;
            DeletedOn = DateTime.Now;
        }

        public virtual void Restore()
        {
            IsDeleted = false;
            DeletedUser = null;
            DeletedOn = null;
        }
    }
}
