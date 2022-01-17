using Construx.App.Domain.Identity;
using Microsoft.AspNetCore.Http;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace Construx.App.Domain.Entities
{
    public class Photo : BaseEntity
    {
        public int ObjectTypeId { get; set; }
        public virtual ObjectType ObjectType { get; set; }
        public int ObjectId { get; set; }
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public string Name { get; set; }
    }
}