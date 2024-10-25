using System.ComponentModel.DataAnnotations;

namespace RSAHyundai.DTOs.Authentication
{
    public class CredentialsDTO : PasswordDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
