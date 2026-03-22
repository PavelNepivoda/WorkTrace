using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class AbsenceType
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
    }
}