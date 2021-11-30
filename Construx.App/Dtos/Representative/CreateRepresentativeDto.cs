using System.ComponentModel.DataAnnotations;

namespace Construx.App.Dtos.Representative
{
    public class CreateRepresentativeDto
    {
        [Required]
        public string IDNP { get; set; }
        [Required]
        public string JobTitle { get; set; }
        [Display(Name = "Company already on website?")]
        public bool CompanyExists { get; set; }
    }
}
