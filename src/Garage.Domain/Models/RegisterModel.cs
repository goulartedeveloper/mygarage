using System.ComponentModel.DataAnnotations;

namespace Garage.Domain.Models
{
    public class RegisterModel
    {
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        public string Username { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(6)]
        [MaxLength(16)]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
