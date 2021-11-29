using Construx.App.Domain.Identity;
using System.Collections.Generic;

namespace Construx.App.Domain.Entities
{
    public class City : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
