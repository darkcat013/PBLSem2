﻿using App.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Domain.Identity
{
    public class User : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime DateCreated { get; set; }
        public virtual ICollection<Bookmark> Bookmarks { get; set; }
        public virtual Representative Representative { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
