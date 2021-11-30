using System.Collections.Generic;

namespace Construx.App.Domain.Entities
{
    public class PlanPartStatus : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<PlanPart> PlanParts { get; set; }
    }
}
