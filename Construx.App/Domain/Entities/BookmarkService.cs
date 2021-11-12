using Construx.App.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class BookmarkService : BaseEntity
    {
        public string ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string Note { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
