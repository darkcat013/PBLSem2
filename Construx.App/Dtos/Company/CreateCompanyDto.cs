using Construx.App.Domain.Entities;
using System.ComponentModel.DataAnnotations;

namespace Construx.App.Dtos.Company
{
    public class CreateCompanyDto
    {
        [Required]
        public string Name { get; set; }
        public string Adress { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [Range(6,13)]
        public string IDNO { get; set; }
        public string Website { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CityId { get; set; }

        public City City { get; set; }
    }
}
