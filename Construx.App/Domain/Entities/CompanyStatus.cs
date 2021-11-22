using System.Collections.Generic;

namespace Construx.App.Domain.Entities
{
    public class CompanyStatus : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Company> Companies { get; set; }
    }
}
