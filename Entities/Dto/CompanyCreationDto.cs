using System.ComponentModel.DataAnnotations;

namespace Entities.Dto
{
    public class CompanyCreationDto
    {
        [Required]
        [MaxLength(60, ErrorMessage = "Maximum length for the Name is 60 characters.")]
        public string Name { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Maximum length for the Address is 100 characters.")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Country is a required field")]
        public string Country { get; set; }
    }
}
