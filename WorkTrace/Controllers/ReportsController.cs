using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WorkTrace.Data;
using WorkTrace.Models;
using WorkTrace.Models.ViewModel;
using WorkTrace.Services;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ReportsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ISettingsService _settings;

        public ReportsController(ApplicationDbContext context, ISettingsService settings)
        {
            _context = context;
            _settings = settings;
        }

        public async Task<IActionResult> Performance(DateTime? from, DateTime? to, Guid? employeeId)
        {
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");

            if (from == null) from = DateTime.Today.AddMonths(-1);
            if (to == null) to = DateTime.Today;

            if (employeeId == null)
                return View(new PerformanceViewModel { StartDate = from.Value, EndDate = to.Value });

            var records = await _context.AttendanceRecords
                .Include(r => r.Employee)
                .ThenInclude(e => e!.ContractType)
                .Where(r => r.EmployeeId == employeeId && r.Date >= from && r.Date <= to && r.StartTime != null && r.EndTime != null)
                .ToListAsync();

            var employee = await _context.Employees
                .Include(e => e.ContractType)
                .FirstOrDefaultAsync(e => e.Id == employeeId);

            if (employee == null || employee.ContractType == null)
            {
                ViewBag.Error = "Zaměstnanec nemá přiřazen typ smlouvy.";
                return View(new PerformanceViewModel { StartDate = from.Value, EndDate = to.Value });
            }

            var totalHours = records.Sum(r => r.GetRoundedWorkHours());
            var hourlyWage = employee.ContractType.HourlyWage;
            var totalWage = (decimal)totalHours * hourlyWage;

            var model = new PerformanceViewModel
            {
                Employee = employee,
                StartDate = from.Value,
                EndDate = to.Value,
                TotalHours = totalHours,
                HourlyWage = hourlyWage,
                TotalWage = totalWage,
                AttendanceRecords = records
            };
            return View(model);
        }

        public async Task<IActionResult> Analysis(DateTime? from, DateTime? to, Guid? employeeId)
        {
            ViewBag.Employees = new SelectList(await _context.Employees.ToListAsync(), "Id", "FullName");

            if (from == null) from = DateTime.Today.AddMonths(-1);
            if (to == null) to = DateTime.Today;

            var query = _context.AttendanceRecords
                .Include(r => r.Employee)
                .ThenInclude(e => e!.ContractType)
                .Where(r => r.Date >= from && r.Date <= to && r.StartTime != null && r.EndTime != null);

            if (employeeId.HasValue)
                query = query.Where(r => r.EmployeeId == employeeId.Value);

            var records = await query.ToListAsync();

            var absencesQuery = _context.Absences
                .Include(a => a.Employee)
                .Where(a => a.Status == AbsenceStatus.Approved && a.StartDate <= to && a.EndDate >= from);

            if (employeeId.HasValue)
                absencesQuery = absencesQuery.Where(a => a.EmployeeId == employeeId.Value);

            var absences = await absencesQuery.ToListAsync();

            var totalWorkHours = records.Sum(r => r.GetRoundedWorkHours());
            var totalWage = records.Sum(r =>
                (decimal)r.GetRoundedWorkHours() *
                (r.Employee?.ContractType?.HourlyWage ?? 0));
            var totalAbsenceDays = absences.Sum(a => (a.EndDate - a.StartDate).TotalDays + 1);
            var workingDayHours = _settings.GetWorkingDayHours();
            var totalAbsenceHours = totalAbsenceDays * workingDayHours;

            var model = new AnalysisViewModel
            {
                StartDate = from.Value,
                EndDate = to.Value,
                TotalWorkHours = totalWorkHours,
                TotalWage = totalWage,
                TotalAbsenceDays = totalAbsenceDays,
                TotalAbsenceHours = totalAbsenceHours,
                AttendanceRecords = records,
                Absences = absences
            };
            return View(model);
        }
    }
}