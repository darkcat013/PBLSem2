using Construx.App.Domain.Identity;
using System.Collections.Generic;

namespace Construx.App.Domain.Entities
{
    public class Plan : BaseEntity
    {
        public int UserId { get; set; }
        public virtual User User { get; set; } 
        public string Name { get; set; } 
        public string Description { get; set; }
        public virtual ICollection<PlanPart> PlanParts { get; set; }
    }
}
