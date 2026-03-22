using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class Absence
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public Guid AbsenceTypeId { get; set; }
        public AbsenceType? AbsenceType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? Reason { get; set; }
        public DateTime RequestedDate { get; set; } = DateTime.Now;
        public AbsenceStatus Status { get; set; } = AbsenceStatus.Pending;
    }

    public enum AbsenceStatus
    {
        Pending,
        Approved,
        Rejected
    }
}