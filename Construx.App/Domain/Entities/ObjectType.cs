using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Construx.App.Domain.Entities
{
    public class ObjectType : BaseEntity
    {
        public string Name { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}