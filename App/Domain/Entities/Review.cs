using App.Domain.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Domain.Entities
{
    public class Review : BaseEntity 
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int ServiceId { get; set; }
        public virtual Service Service { get; set; }
        public int Rating { get; set; }
        public string Description { get; set; }
    }
}
