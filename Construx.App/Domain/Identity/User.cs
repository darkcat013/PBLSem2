using Construx.App.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateCreated { get; set; } = DateTime.Now;
        public virtual ICollection<BookmarkService> BookmarkServices { get; set; }
        public virtual ICollection<BookmarkCompany> BookmarkCompanies { get; set; }
        public virtual Representative Representative { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Plan> Plans { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
    }
}
