namespace WorkTrace.Models.ViewModel
{
    public class PayrollReportVM
    {
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public DateOnly Date { get; set; }
        public TimeSpan? WorkedHours { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal TotalPay { get; set; } 
        public string WorkedHoursFormatted => WorkedHours?.ToString(@"hh\:mm") ?? "N/A";
        public string TotalPayFormatted => TotalPay.ToString("C");
    }
}