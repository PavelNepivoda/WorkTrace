using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class EmployeeRole
    {
        [Key]
        public Guid Id { get; set; }

        [Required, MaxLength(50)]
        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }
    }
}