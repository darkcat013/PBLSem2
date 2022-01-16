using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class Company : BaseEntity
    {
        public string Name { get; set; }
        public string Adress { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string IDNO { get; set; }
        public string Website { get; set; }
        public int StatusId { get; set; }
        public virtual CompanyStatus Status { get; set; }
        public string Description { get; set; }
        public virtual Representative Representative { get; set; }
        public int CityId { get; set; }
        public virtual City City { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual ICollection<Service> Services { get; set; }
    }
}