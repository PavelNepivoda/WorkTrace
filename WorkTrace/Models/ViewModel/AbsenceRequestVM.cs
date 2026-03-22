using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models.ViewModel
{
    public class AbsenceRequestVM
    {
        [Required]
        [StringLength(4, MinimumLength = 4)]
        [Display(Name = "PIN kód")]
        public string PINCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Typ absence")]
        public Guid AbsenceTypeId { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Od data")]
        public DateOnly StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Do data")]
        public DateOnly EndDate { get; set; }

        [Display(Name = "Důvod")]
        public string? Reason { get; set; }
    }
}