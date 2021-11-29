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
        public string IDNO { get; set; }
        public string Website { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public int CityId { get; set; }
    }
}
