using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class ContractType
    {
        [Key]
        public Guid Id { get; set; }
        [Required, MaxLength(50)]
        public string Name { get; set; }
        public string? Description { get; set; }
        [DataType(DataType.Currency)]
        [Display(Name = "Hodinová mzda (Kč)")]
        public decimal HourlyWage { get; set; }
    }
}