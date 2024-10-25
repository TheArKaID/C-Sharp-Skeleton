using System;

namespace RSAHyundai.DTOs.Users
{
    public class UserDetailsDTO
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
