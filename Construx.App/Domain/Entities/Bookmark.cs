using Construx.App.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class Bookmark : BaseEntity
    {
        public int CompanyId { get; set; }
        public int? ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public virtual Company Company { get; set; }
        public int? UserId { get; set; }
        public virtual User User { get; set; }
        public string Note { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
    }
}