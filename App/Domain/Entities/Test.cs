using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Test : BaseEntity
    {
        public string Name { get; set; }
        public DateTime DateCreated { get; set; }
    }
}
