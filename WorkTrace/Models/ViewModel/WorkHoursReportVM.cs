namespace WorkTrace.Models.ViewModel
{
    public class WorkHoursReportVM
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeSpan? WorkedHours { get; set; }  
        public string? StartWork { get; set; }
        public string? EndWork { get; set; }
        public string? BreakStart { get; set; }
        public string? BreakEnd { get; set; }
        public string? BreakDuration { get; set; }  
        public string? WorkedHoursFormatted => WorkedHours?.ToString(@"hh\:mm") ?? "N/A";
    }
}