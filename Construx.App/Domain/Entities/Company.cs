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
        public bool Status { get; set; }
        public string Description { get; set; }
        public virtual Representative Representative { get; set; }
    }
}
