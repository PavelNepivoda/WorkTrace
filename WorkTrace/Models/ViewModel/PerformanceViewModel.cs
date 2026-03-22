using WorkTrace.Models;

namespace WorkTrace.Models.ViewModel
{
    public class PerformanceViewModel
    {
        public Employee? Employee { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalHours { get; set; }
        public decimal HourlyWage { get; set; }
        public decimal TotalWage { get; set; }
        public List<AttendanceRecord> AttendanceRecords { get; set; } = new();
    }
}