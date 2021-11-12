using Construx.App.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class Representative : BaseEntity
    {
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string IDNP { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
    }
}
