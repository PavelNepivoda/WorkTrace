using System.ComponentModel.DataAnnotations;

namespace WorkTrace.Models
{
    public class AttendanceRecord
    {
        [Key]
        public Guid Id { get; set; }
        public Guid EmployeeId { get; set; }
        public Employee? Employee { get; set; }
        public DateTime Date { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? BreakStart { get; set; }
        public DateTime? BreakEnd { get; set; }

        public TimeSpan? TotalWorkTime =>
            (EndTime - StartTime) - (BreakEnd - BreakStart).GetValueOrDefault();
        public double GetRoundedWorkHours()
        {
            if (StartTime == null || EndTime == null)
                return 0;


            var workTime = EndTime.Value - StartTime.Value;


            if (BreakStart.HasValue && BreakEnd.HasValue)
            {
                workTime -= (BreakEnd.Value - BreakStart.Value);
            }

            double totalMinutes = workTime.TotalMinutes;
            double roundedMinutes = Math.Round(totalMinutes / 15) * 15;
            return roundedMinutes / 60;
        }
    }
}