using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WorkTrace.Data;
using WorkTrace.Models;

namespace WorkTrace.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var employees = await _context.Employees.ToListAsync();
            return View(employees);
        }

        [HttpPost]
        public async Task<IActionResult> StartWork(Guid employeeId)
        {
            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(r => r.EmployeeId == employeeId && r.Date == today);

            if (record != null && record.StartTime != null)
            {
                TempData["Error"] = "Pracovní den již byl zahájen.";
            }
            else if (record != null && record.StartTime == null)
            {
                record.StartTime = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pracovní den zahájen.";
            }
            else
            {
                record = new AttendanceRecord
                {
                    Id = Guid.NewGuid(),
                    EmployeeId = employeeId,
                    Date = today,
                    StartTime = DateTime.Now
                };
                _context.AttendanceRecords.Add(record);
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pracovní den zahájen.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> StartBreak(Guid employeeId)
        {
            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(r => r.EmployeeId == employeeId && r.Date == today);

            if (record == null || record.StartTime == null)
            {
                TempData["Error"] = "Nejprve zahajte pracovní den.";
            }
            else if (record.BreakStart != null && record.BreakEnd == null)
            {
                TempData["Error"] = "Již máte započatou přestávku.";
            }
            else
            {
                record.BreakStart = DateTime.Now;
                record.BreakEnd = null;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Přestávka začala.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EndBreak(Guid employeeId)
        {
            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(r => r.EmployeeId == employeeId && r.Date == today);

            if (record == null || record.StartTime == null)
            {
                TempData["Error"] = "Nejprve zahajte pracovní den.";
            }
            else if (record.BreakStart == null || record.BreakEnd != null)
            {
                TempData["Error"] = "Přestávka není aktivní.";
            }
            else
            {
                record.BreakEnd = DateTime.Now;
                await _context.SaveChangesAsync();
                TempData["Success"] = "Přestávka ukončena.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> EndWork(Guid employeeId)
        {
            var today = DateTime.Today;
            var record = await _context.AttendanceRecords
                .FirstOrDefaultAsync(r => r.EmployeeId == employeeId && r.Date == today);

            if (record == null || record.StartTime == null)
            {
                TempData["Error"] = "Nejprve zahajte pracovní den.";
            }
            else if (record.EndTime != null)
            {
                TempData["Error"] = "Pracovní den již byl ukončen.";
            }
            else
            {
                record.EndTime = DateTime.Now;
                if (record.BreakStart != null && record.BreakEnd == null)
                {
                    record.BreakEnd = DateTime.Now;
                }
                await _context.SaveChangesAsync();
                TempData["Success"] = "Pracovní den ukončen.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> RequestAbsence(Guid employeeId, Guid absenceTypeId, DateTime startDate, DateTime endDate, string? reason)
        {
            var absence = new Absence
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                AbsenceTypeId = absenceTypeId,
                StartDate = startDate,
                EndDate = endDate,
                Reason = reason,
                RequestedDate = DateTime.Now,
                Status = AbsenceStatus.Pending
            };
            _context.Absences.Add(absence);
            await _context.SaveChangesAsync();
            TempData["Success"] = "Žádost o absenci odeslána.";
            return RedirectToAction(nameof(Index));
        }
    }
}