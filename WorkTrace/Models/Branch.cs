using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class Branch
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Address { get; set; }
    }
}