using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models.ViewModel
{
    public class AttendanceEntryVM
    {
        [Required]
        [StringLength(4, MinimumLength = 4)]
        [Display(Name = "PIN kód")]
        public string PINCode { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Datum")]
        public DateTime SelectedDate { get; set; } = DateTime.Today;

        public bool IsToday => SelectedDate.Date == DateTime.Today;

        public string? Action { get; set; }

        public string? EmployeeName { get; set; }
        public DateTime? StartWork { get; set; }
        public DateTime? StartBreak { get; set; }
        public DateTime? EndBreak { get; set; }
        public DateTime? EndWork { get; set; }
    }
}