using System.Collections.Generic;

namespace Construx.App.Domain.Entities
{
    public class PlanPart : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public virtual Plan Part { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}
