using System.ComponentModel.DataAnnotations;

namespace Entities.Dto
{
    public class EmployeeForManInpulationDto
    {
        [Required(ErrorMessage = "Employee name is a required field")]
        [MaxLength(50, ErrorMessage = "Maximum length for the First Name is 50 characters")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Employee last name is a required field")]
        [MaxLength(50, ErrorMessage = "Maximum length for the Last Name is 50 characters")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Employee age is a required field")]
        [Range(1, 200, ErrorMessage = "Age must be between 18 and 65")]
        public int Age { get; set; }
        public string Position { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
    }
}
