using System.ComponentModel.DataAnnotations;

namespace RSAHyundai.DTOs.Tasks
{
    public class TaskDTO
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
