using System.ComponentModel.DataAnnotations;

namespace Gapplus.Application.DTO.User.Request
{
    public class UpdateUserDto
    {
        [Required]
        public string? UserFirstName { get; set; }

        [Required]
        public string? UserLastName { get; set; }

        [Required]
        public string? UserName { get; set; }

        [Required]
        [EmailAddress]
        public string? UserEmail { get; set; }

        [Required]
        [RegularExpression(@"^\+[1-9]\d{1,14}$")] // Regex for international phone number
        public string? UserPhoneNumber { get; set; }

        [Required]
        public string? UserPassword { get; set; }

        [Required]
        [Compare("UserPassword")]
        public string? ConfirmUserPassword { get; set; }
    }
}
