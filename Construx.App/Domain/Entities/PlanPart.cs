using System;
using System.Collections.Generic;

namespace Construx.App.Domain.Entities
{
    public class PlanPart : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime FromDate { get; set; }
        public DateTime ToDate { get; set; }
        public int StatusId { get; set; }
        public virtual PlanPartStatus Status { get; set; }
        public int PlanId { get; set; }
        public virtual Plan Plan { get; set; }
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public int Priority { get; set; }
    }
}
