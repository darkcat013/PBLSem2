using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class Service : BaseEntity
    {
        public int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Rating { get; set; } = 0;
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<PlanPart> PlanParts { get; set; }
    }
}