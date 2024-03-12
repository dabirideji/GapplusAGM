using System.ComponentModel.DataAnnotations;

namespace Gapplus.Application.DTO.User.Request
{
    public class UserLoginDto
    {
        [Required]
        public string? UserNameOrEmail { get; set; }

        [Required]
        public string? UserPassword { get; set; }
    }
}
