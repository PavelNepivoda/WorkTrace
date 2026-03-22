using WorkTrace.Models;

namespace WorkTrace.Models.ViewModel
{
    public class AnalysisViewModel
    {
        public Employee? Employee { get; set; }
        public Guid? EmployeeId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public double TotalWorkHours { get; set; }
        public decimal TotalWage { get; set; }
        public double TotalAbsenceDays { get; set; }
        public double TotalAbsenceHours { get; set; }
        public List<AttendanceRecord> AttendanceRecords { get; set; } = new();
        public List<Absence> Absences { get; set; } = new();
    }
}