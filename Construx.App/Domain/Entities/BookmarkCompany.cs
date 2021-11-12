using Construx.App.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class BookmarkCompany : BaseEntity
    {
        public string CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public string UserId { get; set; }
        public virtual User User { get; set; }
        public string Note { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<BookmarkCompany> Bookmarks { get; set; }
    }
}
